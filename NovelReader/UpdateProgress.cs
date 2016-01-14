using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace NovelReader
{
    public class UpdateDataGridViewProgressColumn : DataGridViewImageColumn
    {
        public UpdateDataGridViewProgressColumn()
        {
            CellTemplate = new UpdateDataGridViewProgressCell();
        }
    }

    class UpdateDataGridViewProgressCell : DataGridViewImageCell
    {
        // Used to make custom cell consistent with a DataGridViewImageCell
        static Image emptyImage;
        static UpdateDataGridViewProgressCell()
        {
            emptyImage = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }
        public UpdateDataGridViewProgressCell()
        {
            this.ValueType = typeof(Tuple<int, string>);
        }
        // Method required to make the Progress Cell consistent with the default Image Cell. 
        // The default Image Cell assumes an Image as a value, although the value of the Progress Cell is an int.
        protected override object GetFormattedValue(object value,
                            int rowIndex, ref DataGridViewCellStyle cellStyle,
                            TypeConverter valueTypeConverter,
                            TypeConverter formattedValueTypeConverter,
                            DataGridViewDataErrorContexts context)
        {
            return emptyImage;
        }

        protected override void Paint(System.Drawing.Graphics g, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            Tuple<int, string> updateInfo = (Tuple<int, string>)value;
            int progressVal = updateInfo.Item1;
            string message = updateInfo.Item2;

            float percentage = ((float)progressVal / 100.0f); // Need to convert to float before division; otherwise C# returns int which is 0 for anything but 100%.
            Brush backColorBrush = new SolidBrush(cellStyle.BackColor);
            Brush foreColorBrush = new SolidBrush(cellStyle.ForeColor);
            // Draws the cell grid
            base.Paint(g, clipBounds, cellBounds,
             rowIndex, cellState, value, formattedValue, errorText,
             cellStyle, advancedBorderStyle, (paintParts & ~DataGridViewPaintParts.ContentForeground));


            if (percentage > 0.0)
            {
                // Draw the progress bar and the text
                g.FillRectangle(new SolidBrush(Color.FromArgb(163, 189, 242)), cellBounds.X + 2, cellBounds.Y + 2, Convert.ToInt32((percentage * cellBounds.Width - 4)), cellBounds.Height - 4);
                g.DrawString(message, cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 3);
            }
            else
            {
                // draw the text
                if (this.DataGridView.SelectedRows.Count > 0 && this.DataGridView.SelectedRows[0].Index == rowIndex)
                    g.DrawString(message, cellStyle.Font, new SolidBrush(cellStyle.SelectionForeColor), cellBounds.X + 6, cellBounds.Y + 3);

                else
                    g.DrawString(message, cellStyle.Font, foreColorBrush, cellBounds.X + 6, cellBounds.Y + 3);

            }
        }
    }
}
