#region License Information (GPL v3)

/*
    VIrecord - A program that allows you to take screenshots and share any file type
    Copyright (c) 2007-2026 VIrecord Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using System.Drawing;
using System.Windows.Forms;

namespace VIrecord.HelpersLib
{
    public class DarkColorTable : ProfessionalColorTable
    {
        public override Color ButtonSelectedHighlight => VIrecordResources.Theme.MenuHighlightColor;
        public override Color ButtonSelectedHighlightBorder => VIrecordResources.Theme.MenuHighlightBorderColor;
        public override Color ButtonPressedHighlight => VIrecordResources.Theme.MenuHighlightColor;
        public override Color ButtonPressedHighlightBorder => VIrecordResources.Theme.MenuHighlightBorderColor;
        public override Color ButtonCheckedHighlight => VIrecordResources.Theme.MenuCheckBackgroundColor;
        public override Color ButtonCheckedHighlightBorder => VIrecordResources.Theme.MenuHighlightBorderColor;
        public override Color ButtonPressedBorder => VIrecordResources.Theme.MenuHighlightBorderColor;
        public override Color ButtonSelectedBorder => VIrecordResources.Theme.MenuHighlightBorderColor;
        public override Color ButtonCheckedGradientBegin => VIrecordResources.Theme.MenuCheckBackgroundColor;
        public override Color ButtonCheckedGradientMiddle => VIrecordResources.Theme.MenuCheckBackgroundColor;
        public override Color ButtonCheckedGradientEnd => VIrecordResources.Theme.MenuCheckBackgroundColor;
        public override Color ButtonSelectedGradientBegin => VIrecordResources.Theme.MenuHighlightColor;
        public override Color ButtonSelectedGradientMiddle => VIrecordResources.Theme.MenuHighlightColor;
        public override Color ButtonSelectedGradientEnd => VIrecordResources.Theme.MenuHighlightColor;
        public override Color ButtonPressedGradientBegin => VIrecordResources.Theme.MenuHighlightColor;
        public override Color ButtonPressedGradientMiddle => VIrecordResources.Theme.MenuHighlightColor;
        public override Color ButtonPressedGradientEnd => VIrecordResources.Theme.MenuHighlightColor;
        public override Color CheckBackground => VIrecordResources.Theme.MenuCheckBackgroundColor;
        public override Color CheckSelectedBackground => VIrecordResources.Theme.MenuCheckBackgroundColor;
        public override Color CheckPressedBackground => VIrecordResources.Theme.MenuCheckBackgroundColor;
        public override Color GripDark => VIrecordResources.Theme.SeparatorDarkColor;
        public override Color GripLight => VIrecordResources.Theme.SeparatorLightColor;
        public override Color ImageMarginGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color ImageMarginGradientMiddle => VIrecordResources.Theme.BackgroundColor;
        public override Color ImageMarginGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color ImageMarginRevealedGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color ImageMarginRevealedGradientMiddle => VIrecordResources.Theme.BackgroundColor;
        public override Color ImageMarginRevealedGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color MenuStripGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color MenuStripGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color MenuItemSelected => VIrecordResources.Theme.MenuHighlightColor;
        public override Color MenuItemBorder => VIrecordResources.Theme.MenuBorderColor;
        public override Color MenuBorder => VIrecordResources.Theme.MenuBorderColor;
        public override Color MenuItemSelectedGradientBegin => VIrecordResources.Theme.MenuHighlightColor;
        public override Color MenuItemSelectedGradientEnd => VIrecordResources.Theme.MenuHighlightColor;
        public override Color MenuItemPressedGradientBegin => VIrecordResources.Theme.MenuHighlightColor;
        public override Color MenuItemPressedGradientMiddle => VIrecordResources.Theme.MenuHighlightColor;
        public override Color MenuItemPressedGradientEnd => VIrecordResources.Theme.MenuHighlightColor;
        public override Color RaftingContainerGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color RaftingContainerGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color SeparatorDark => VIrecordResources.Theme.SeparatorDarkColor;
        public override Color SeparatorLight => VIrecordResources.Theme.SeparatorLightColor;
        public override Color StatusStripGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color StatusStripGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripBorder => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripDropDownBackground => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripGradientMiddle => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripContentPanelGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripContentPanelGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripPanelGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color ToolStripPanelGradientEnd => VIrecordResources.Theme.BackgroundColor;
        public override Color OverflowButtonGradientBegin => VIrecordResources.Theme.BackgroundColor;
        public override Color OverflowButtonGradientMiddle => VIrecordResources.Theme.BackgroundColor;
        public override Color OverflowButtonGradientEnd => VIrecordResources.Theme.BackgroundColor;
    }
}