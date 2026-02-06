using EPiServer.Cms.TinyMce.Core;

namespace DemoTraining.Extensions
{
    public static class TinyMceConfigurationExtensions
    {
        public static void RichtextExtension(this TinyMceConfiguration config)
        {
            TinyMceSettings defaultSettings = config.Default();

            // Add the advanced list styles and code plugins and append 
            // tool buttons for paste and source code to the toolbar.
            defaultSettings
                .AddPlugin("advlist")
                .AddPlugin("code").AppendToolbar("code")
                .AppendToolbar("paste");

            // Change the dimensions of the Source Code dialog to be tall and skinny.
            defaultSettings
                .AddSetting("code_dialog_height", 400)
                .AddSetting("code_dialog_width", 100);

            // Improve entity encoding compatibility.
            defaultSettings.AddSetting("entity_encoding", "numeric");

            // Activate emoticons and charmap with two extra chars.
            defaultSettings
                .AddPlugin("charmap emoticons")
                .AppendToolbar("charmap emoticons | removeformat")
                .AddSetting("charmap_append", new[]
                {
            new object[] { 9861, "Dice number 6" },
            new object[] { 9925, "Sun behind cloud" }
                });

            // Add table plugin and insert tool after link tool.
            defaultSettings
                .AddPlugin("table")
                .InsertTool("table", after: "epi-link");

            // default is "tableprops tabledelete | tableinsertrowbefore tableinsertrowafter 
            // tabledeleterow | tableinsertcolbefore tableinsertcolafter tabledeletecol"

            defaultSettings.AddSetting("table_toolbar",
                "tabledelete | tableinsertrowbefore tableinsertrowafter tabledeleterow | tableinsertcolbefore tableinsertcolafter tabledeletecol")

                // the following will not appear: Cell spacing, Cell padding, Border and Caption
                .AddSetting("table_appearance_options", false)

                // Advanced tab allows a user to input style, border color and background color 
                // values.  Hide the Advanced tab like this:
                .AddSetting("table_advtab", false)
                .AddSetting("table_row_advtab", false)
                .AddSetting("table_cell_advtab", false);

            // CMS 12
            defaultSettings.ContentCss("/css/style.css");

            // CMS 11 and CMS 12
            // Use a custom extension method to insert after block formats.
            defaultSettings.InsertTool("styleselect", after: "formatselect");

            // Customize the Formats dropdown list.
            defaultSettings
                .StyleFormats(
                    new { title = "Red text", inline = "span", styles = new { color = "#ff0000" } },
                    new { title = "Awesome numbering", selector = "ol", classes = "awesome-numbering" },
                    new { title = "Roman numbering", selector = "ol", classes = "roman-numbering" }
                );

            defaultSettings.InsertTool("fontsizeselect", after: "styleselect");

            defaultSettings.OutputToDebug("default after");
        }
    }
}
