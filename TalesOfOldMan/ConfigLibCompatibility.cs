using System.Numerics;
using ConfigLib;
using ImGuiNET;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace TalesOfOldMan;

/// <summary>
/// Provides ConfigLib integration for TalesOfOldMan mod.
/// </summary>
public class ConfigLibCompatibility
{

    /// <summary>
    /// Registers the TalesOfOldMan config screen with ConfigLib.
    /// </summary>
    public ConfigLibCompatibility(ICoreAPI api)
    {
        api.ModLoader.GetModSystem<ConfigLibModSystem>().RegisterCustomConfig(TalesOfOldManModSystem.ModId, (_, buttons) =>
        {
            if (buttons.Save) api.StoreModConfig<ModConfig>(ModConfig.Instance, TalesOfOldManModSystem.ConfigFileName);
            if (buttons.Restore) ModConfig.Instance.ItemHighlighter = api.LoadModConfig<ItemHighlighterConfig>(TalesOfOldManModSystem.ConfigFileName);
            if (buttons.Defaults) ModConfig.Instance.ItemHighlighter = new();
            BuildSettings(ModConfig.Instance);
        });
    }

    private void BuildSettings(ModConfig config)
    {
        BuildClientSideSettings(config);
        BuildServerSideSettings(config);
    }

    private void BuildServerSideSettings(ModConfig config)
    {
        if (ImGui.CollapsingHeader("Server Side Configurations"))
        {
            ImGui.Indent();
            ItemInteractionsSettings(config.ItemInteractions);
            ImGui.Unindent();
        }
    }

    private void BuildClientSideSettings(ModConfig config)
    {
        if (ImGui.CollapsingHeader("Client Only Features"))
        {
            ImGui.Indent();
            ItemHighlighterSettings(config.ItemHighlighter);
            ImGui.Unindent();
        }
    }


    private void ItemHighlighterSettings(ItemHighlighterConfig config)
    {
        if (ImGui.CollapsingHeader("Item Highlighter"))
        {
            ImGui.Indent();

            // Feature Toggle
            var featureEnabled = config.Enabled;
            if (ImGui.Checkbox("Enabled", ref featureEnabled))
            {
                config.Enabled = featureEnabled;
            }
            TextPropertyDescription("Enable/Disable this feature.");

            // Highlight Distance
            ImGui.Text("Highlight distance (blocks).");
            ImGui.PushItemWidth(200);
            var highlightDistance = config.HighlightDistance;
            if (ImGui.SliderInt("##HighlightDistance", ref highlightDistance, 2, 50, "%d"))
            {
                config.HighlightDistance = highlightDistance;
            }
            ImGui.PopItemWidth();
            TextPropertyDescription("How far away items can be highlighted.");

            ImGui.Separator();

            // Continuous Mode
            var continuousMode = config.HighlightContinousMode;
            if (ImGui.Checkbox("Continuous Highlight Mode", ref continuousMode))
            {
                config.HighlightContinousMode = continuousMode;
            }
            TextPropertyDescription("If enabled, items are highlighted continuously without pressing the hotkey.");

            // Show Item Names
            var showItemNames = config.ShowItemNames;
            if (ImGui.Checkbox("Display Item Names (when Sneaking)", ref showItemNames))
            {
                config.ShowItemNames = showItemNames;
            }
            TextPropertyDescription("If enabled, items are highlighted continuously without pressing the hotkey.");

            ImGui.Unindent();
        }

        if (ImGui.CollapsingHeader("Highlighter Appearance"))
        {
            ImGui.Indent();

            // Highlight Color
            ImGui.Text("Highlight color.");
            ImGui.PushItemWidth(200);
            var highlightColor = new Vector4(ColorUtil.ToRGBAFloats(config.HighlightColor)) ;
            if (ImGui.ColorPicker4("##HighlightColor", ref highlightColor))
            {
                config.HighlightColor = ColorUtil.FromRGBADoubles(new double[] { highlightColor.X, highlightColor.Y, highlightColor.Z, highlightColor.W });
            }
            ImGui.PopItemWidth();
            TextPropertyDescription("The color of the highlight.");
            ImGui.Unindent();
        }
    }


    private void ItemInteractionsSettings(WorldInteractionsConfigs config)
    {
        if (ImGui.CollapsingHeader("World Interactions"))
        {
            ImGui.Indent();


            // Continuous Mode
            var permissiveRightClick = config.PermissiveRightClick;
            if (ImGui.Checkbox("Permissive Right Click", ref permissiveRightClick))
            {
                config.PermissiveRightClick = permissiveRightClick;
            }

            TextPropertyDescription("If enabled, you can pickup ground items like loose stones and flint even with busy hands");
            ImGui.Unindent();
        }
    }

    private void TextPropertyDescription(string text)
    {
        ImGui.BeginDisabled();
        ImGui.TextWrapped(text);
        ImGui.EndDisabled();
    }
} 