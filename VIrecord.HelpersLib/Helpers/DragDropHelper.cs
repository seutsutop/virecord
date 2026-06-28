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

using System;
using System.Windows.Forms;

namespace VIrecord.HelpersLib
{
    public static class DragDropHelper
    {
        public static void HandleDragEnter(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        public static string[] GetDroppedFiles(DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) && e.Data.GetData(DataFormats.FileDrop, false) is string[] files)
            {
                return files;
            }

            return null;
        }

        public static string GetFirstDroppedFile(DragEventArgs e)
        {
            string[] files = GetDroppedFiles(e);

            if (files != null && files.Length > 0)
            {
                return files[0];
            }

            return null;
        }
    }
}
