using EPiServer.Cms.TinyMce.Core;
using System.Diagnostics;

namespace DemoTraining.Extensions
{
    public static class TinyMceSettingsExtensions
    {
        public static void OutputToDebug(this TinyMceSettings settings, string heading)
        {
            Debug.WriteLine($"TinyMCE settings - {heading}");

            foreach (var item in settings)
            {
                Debug.WriteLine($"{item.Key}: {item.Value}");

                if (typeof(string[]) == item.Value.GetType())
                {
                    foreach (string entry in item.Value as string[])
                    {
                        Debug.WriteLine($"  {entry}");
                    }
                }
            }
        }

        public static void InsertTool(this TinyMceSettings settings, string tool, string after, int row = 0)
        {
            // Get the toolbar from the settings object.
            string[] toolbar = settings["toolbar"] as string[];

            if (toolbar == null) return;

            // Get the toolbar row requested (or first row by default)
            string toolbarRow = toolbar[row];

            // Find the position of the tool to insert after.
            int position = toolbarRow.IndexOf(after) + after.Length;

            // Insert the new tool at the position with spacing.
            string newRow = toolbarRow.Insert(position, $" {tool}");

            // Replace the row.
            toolbar[row] = newRow;

            // Replace the toolbar.
            settings.Toolbar(toolbar);
        }
    }
}
