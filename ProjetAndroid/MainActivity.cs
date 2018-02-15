using System;
using Android.App;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using System.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using ModernHttpClient;
using Newtonsoft.Json;

namespace ProjetAndroid
{
    [Activity(Label = "ProjetAndroid", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        // on instencie la liste des produits
        public List<Produit> produit { get; private set; }

        // methode de création et d'appel de la fenetre principale
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // apple de la ressource layout main.axml
            SetContentView(Resource.Layout.Main);

            // on déclare les obejts bouton et champs de saisie présents dans le layout aux ressources exitantes
            Button button = FindViewById<Button>(Resource.Id.myButton);
            EditText produit = FindViewById<EditText>(Resource.Id.produitText);

            // on affecte au clic sur le bouton la méthode de recherche du produit en passant en argument 
            // le champs de saisie contenant le numéro de produit
            // on utilise la fonction delegate car le fonctionnement de l'appel de la méthode se fait de facon 
            // asynchrone
            button.Click += delegate { EditTaskAsync(produit.Text); };
            {
            };
       
        }

        public Task EditTaskAsync(string num_produit)
        {
           // recherche du produit
           return RechercheProduit(num_produit);          
        }


        private async Task<List<Produit>> RechercheProduit(string num_produit)
        {
            // on declare les elements graphiques
            TextView etatProduit = FindViewById<TextView>(Resource.Id.etatProduit);
            TextView typeProduit = FindViewById<TextView>(Resource.Id.typeProduit);
            TextView poidsProduit = FindViewById<TextView>(Resource.Id.poidsProduit);

            // on vide les elements graphiques avant de les rmplir
            typeProduit.Text = "";
            etatProduit.Text = "";
            poidsProduit.Text = "";

            // delcaration du client http
            HttpClient client = new HttpClient(new ModernHttpClient.NativeMessageHandler());
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             

            try
            {

                // affichage de la popup utilisateur
                Toast.MakeText(this, "Recherche en cours ", ToastLength.Long).Show();

                // affectation du numero de bobine a rechercher à l'url de l'application
                string uri = Constants.RestUrl + num_produit;

                // envoie de la commande GET au service webapi
                string response = await client.GetStringAsync(uri);

                // deserialisation de la reponse venant du serveur
                var t = JsonConvert.DeserializeObject<Produit>(response);

                // affichage de la popup utilisateur
                Toast.MakeText(this, "Les données ont été trouvées :-) ", ToastLength.Long).Show();

                // on affiche les données dans les elements graphiques
                typeProduit.Text =  t.Type_Produit_ID;
                etatProduit.Text =  t.Etat;
                poidsProduit.Text = t.Poids.ToString();
            }
            catch (Exception ex)
            {
                // affichage de la popup utilisateur
                Toast.MakeText(this, "ERROR : " + ex.Message, ToastLength.Long).Show();
            }

            return null;
        }
    }
}


