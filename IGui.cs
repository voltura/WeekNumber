namespace WeekNumber
{
    /// <summary>
    /// GUI Interface
    /// </summary>
    public interface IGui
    {
        /// <summary>
        /// Updates icon on GUI with given week number
        /// </summary>
        /// <param name="weekNumber">The week number to display on icon</param>
        /// <param name="iconResolution">The width and height of the icon</param>
        /// <param name="redrawContextMenu">Redraw context menu</param>
        void UpdateIcon(int weekNumber, int iconResolution = (int)IconSize.Icon256, bool redrawContextMenu = false);

        /// <summary>
        /// Disposes GUI
        /// </summary>
        void Dispose();

        /// <summary>
        /// Event handler for when GUI (icon) update is requested
        /// </summary>
        event System.EventHandler UpdateRequest;
    }
}