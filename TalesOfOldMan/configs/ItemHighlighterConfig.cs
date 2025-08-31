using Vintagestory.API.MathTools;

namespace TalesOfOldMan
{
    class ItemHighlighterConfig
    {
        /// <summary>
        /// Enabled/Disable Feature
        /// </summary>
        public bool Enabled { get; set; } = false;

        /// <summary>
        /// Highlight distance.
        /// </summary>
        public int HighlightDistance { get { return _highlightDistance; } set { _highlightDistance = value >= 2 ? value : 2; } }
        private int _highlightDistance = 10;

        /// <summary>
        /// Highlight Continous Mode.
        /// </summary>
        private bool _continousMode = false;
        public bool HighlightContinousMode { get { return Enabled && _continousMode; } set { _continousMode = value; } }

        /// <summary>
        /// Highlight Color.
        /// </summary>
        public int HighlightColor { get; set; } = ColorUtil.WhiteArgb;

        /// <summary>
        /// Enabled/Disable Feature
        /// </summary>
        private bool _showItemNames = false;
        public bool ShowItemNames { get { return Enabled && _showItemNames; } set { _showItemNames = value; } }
    }
}