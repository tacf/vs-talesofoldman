using System;
using System.Linq;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace TalesOfOldMan;

public static class Block_OnBlockInteractStart_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(Block).GetMethod(nameof(Block.OnBlockInteractStart));
    }

    public static MethodInfo GetPostfix() => typeof(Block_OnBlockInteractStart_Patch).GetMethod(nameof(Postfix));

    /// <summary>
    /// Handle ground storage liquid interactions
    /// </summary>
    /// <returns>Return false to skip original method</returns>
    public static bool Postfix(bool result, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
    {
        // Check if feature enabled
        if (!ModConfig.Instance.ItemInteractions.PermissiveRightClick) return result;

        // Do nothing if no block selection is received or previous action was successful
        if (blockSel == null || blockSel.Block == null) return result;
        if (result) return result;
        if (!blockSel.Block.HasBehavior<BlockBehaviorRightClickPickup>()) return result;

        bool pickedUp = false;
        AssetLocation pickUpsound = null;

        if (!byPlayer.Entity.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
        {
            // Handle world claims
            world.BlockAccessor.MarkBlockDirty(blockSel.Position.AddCopy(blockSel.Face));
            byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
            return pickedUp;
        }
        foreach (var drop in blockSel.Block.Drops)
        {
            var itemStack = drop.GetNextItemStack();
            if (byPlayer.InventoryManager.TryGiveItemstack(itemStack))
            {
                pickedUp = true;
                pickUpsound = itemStack.Item.GetBehavior<CollectibleBehaviorGroundStorable>()?.StorageProps.PlaceRemoveSound;
                world.Logger.Audit("[{0} - Permissive Right Click Pickup] {1} Took 1x{2} from block at {3}.",
                    TalesOfOldManModSystem.ModId,
                    byPlayer.PlayerName,
                    itemStack.Collectible.Code,
                    blockSel.Position
                );
            }
        }
        
        if (pickedUp)
        {
            AssetLocation breakSound = blockSel.Block.GetSounds(world.BlockAccessor, blockSel).GetBreakSound(byPlayer);
            world.PlaySoundAt(pickUpsound == null ? breakSound : pickUpsound, byPlayer, byPlayer);
            world.BlockAccessor.SetBlock(0, blockSel.Position);
        }
        return pickedUp;
    }
}