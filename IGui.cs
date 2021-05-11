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
        /// <param name="weekNumber"></param>
        void UpdateIcon(int weekNumber);

        /// <summary>
        /// Disposes GUI
        /// </summary>
        void Dispose();
    }
}