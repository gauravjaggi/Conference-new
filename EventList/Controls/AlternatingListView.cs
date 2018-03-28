using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EventList.Controls
{
    public class AlternatingListView : ListView
    {

        protected override void SetupContent(Cell content, int index)
        {
            base.SetupContent(content, index);

            var viewCell = content as ViewCell;
            viewCell.View.BackgroundColor = index % 2 == 0 ? Color.FromHex("#9de2e1") : Color.FromHex("#8bd7f6");
        }
    }
}
