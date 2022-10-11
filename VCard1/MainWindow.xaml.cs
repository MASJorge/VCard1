using Microsoft.Win32;
using MixERP.Net.VCards;
using MixERP.Net.VCards.Models;
using MixERP.Net.VCards.Serializer;
using MixERP.Net.VCards.Types;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace VCard1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // fijarse en https://github.com/mixerp/MixERP.Net.VCards/blob/master/src/MixERP.Net.VCards.UI/SampleVCards.cs
        private void GuardarVCard_Click(object sender, RoutedEventArgs e)
        {
            var vcard = new VCard
            {
                Version = VCardVersion.V4,
                FormattedName = txtNombre.Text + " " + txtApellido.Text,
                FirstName = txtNombre.Text,
                LastName = txtApellido.Text,

                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Newfoundland Standard Time"),
                DeliveryAddress = new DeliveryAddress
                {
                    Type = AddressType.Parcel,
                    Address = txtDireccion.Text,
                },
                Photo = GetPhoto(),
                Addresses = new List<Address>
                    {
                           new Address
                        {
                            Type = AddressType.Home,
                            Street = txtDireccion.Text,
                            Locality = "Las Heras",
                            Region = "Mendoza",
                            Country = "Argentina",
                            TimeZone = TimeZoneInfo.Local
                        }
                          
                    },
                Telephones = new List<Telephone>
                    {
                        new Telephone
                        {
                            Type = TelephoneType.Preferred,
                            Number = txtTelefono.Text,
                        },
                        new Telephone
                        {
                            Type = TelephoneType.Home,
                            Number = txtTelefono.Text,
                        },
                        new Telephone
                        {
                            Type = TelephoneType.Work,
                            Number = txtTelefono.Text,
                        },
   
                        new Telephone
                        {
                            Type = TelephoneType.Cell,
                            Number = txtTelefono.Text,
                        }
                    },
                Emails = new List<Email>
                    {
                        new Email
                        {
                            Type = EmailType.Smtp,
                            EmailAddress = txtMail.Text,
                        },
                    },
                Note = "This is a test",
                Organization = "VirtusWay",
                Classification = ClassificationType.Public,
                OrganizationalUnit = "Argentina, Division;Developers",
                Role = "Developer",
                Kind = Kind.Individual,
            };

            string serialized = vcard.Serialize();
            string path = Path.Combine("C:\\Users\\JorgeArielMas\\source\\repos", vcard.FormattedName + ".vcf");
            File.WriteAllText(path, serialized);
        }

        private string base64;

        private void CargarFoto_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos PNG (*.PNG)|*.PNG";

            if(openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;

                Uri fileUri = new Uri(path);
                photo.Source = new BitmapImage(fileUri);
                

                using var fileStream = File.OpenRead(path);
                using MemoryStream ms = new MemoryStream();
                fileStream.CopyTo(ms);
                byte[] imageBytes = ms.ToArray();

                base64 = Convert.ToBase64String(imageBytes);

            }
        }
        private Photo GetPhoto()
        {
            return new Photo(true, "png", base64);
        }

    }
}
