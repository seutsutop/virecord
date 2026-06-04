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

using VIrecord.HelpersLib;
using VIrecord.ImageEffectsLib.Properties;
using System;
using System.IO;
using System.Windows.Forms;

namespace VIrecord.ImageEffectsLib
{
    public partial class ImageEffectPackagerForm : Form
    {
        public string ImageEffectJson { get; private set; }
        public string ImageEffectName { get; private set; }
        public string VIrecordImageEffectsFolderPath { get; private set; }
        public string AssetsFolderPath { get; set; }
        public string PackageFilePath { get; set; }

        public ImageEffectPackagerForm(string json, string name, string imageEffectsFolderPath)
        {
            ImageEffectJson = json;
            ImageEffectName = name;
            VIrecordImageEffectsFolderPath = imageEffectsFolderPath;

            InitializeComponent();
            VIrecordResources.ApplyTheme(this, true);

            AssetsFolderPath = Path.Combine(VIrecordImageEffectsFolderPath, ImageEffectName);
            txtAssetsFolderPath.Text = AssetsFolderPath;
            PackageFilePath = AssetsFolderPath + ".sxie";
            txtPackageFilePath.Text = PackageFilePath;
        }

        private void btnOpenImageEffectsFolder_Click(object sender, EventArgs e)
        {
            FileHelpers.OpenFolder(VIrecordImageEffectsFolderPath);
        }

        private void txtAssetsFolderPath_TextChanged(object sender, EventArgs e)
        {
            AssetsFolderPath = txtAssetsFolderPath.Text;
        }

        private void btnAssetsFolderPathBrowse_Click(object sender, EventArgs e)
        {
            FileHelpers.BrowseFolder(txtAssetsFolderPath, VIrecordImageEffectsFolderPath);
        }

        private void txtPackageFilePath_TextChanged(object sender, EventArgs e)
        {
            PackageFilePath = txtPackageFilePath.Text;
        }

        private void btnPackageFilePathBrowse_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = "sxie";
                sfd.FileName = ImageEffectName + ".sxie";
                sfd.Filter = "VIrecord image effect (*.sxie)|*.sxie";
                sfd.InitialDirectory = VIrecordImageEffectsFolderPath;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    txtPackageFilePath.Text = sfd.FileName;
                }
            }
        }

        private void btnPackage_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(AssetsFolderPath) && !AssetsFolderPath.StartsWith(VIrecordImageEffectsFolderPath + "\\", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(Resources.AssetsFolderMustBeInsideVIrecordImageEffectsFolder, "VIrecord - " + Resources.InvalidAssetsFolderPath,
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (!File.Exists(PackageFilePath) || MessageBox.Show(Resources.PackageWithThisFileNameAlreadyExistsRNWouldYouLikeToOverwriteIt, "VIrecord",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string outputFilePath = ImageEffectPackager.Package(PackageFilePath, ImageEffectJson, AssetsFolderPath);

                    if (!string.IsNullOrEmpty(outputFilePath) && File.Exists(outputFilePath))
                    {
                        FileHelpers.OpenFolderWithFile(outputFilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ShowError();
            }
        }
    }
}