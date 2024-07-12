using Android.Views;
using Firebase.Database;
using Newtonsoft.Json;

namespace HealthVisualization.Activities
{
    [Activity(Label = "CadastroUsuarioActivity")]
    public class CadastroUsuarioActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_cadastro_user); 

            Button cadastrarUsuarioButton = FindViewById<Button>(Resource.Id.buttonCadastrarUser);
            if (cadastrarUsuarioButton != null)
            {
                cadastrarUsuarioButton.Click += CadastraUsuarioAsync;
            }
        }
        private async void CadastraUsuarioAsync(object sender, EventArgs e)
        {
            var nomeUser = FindViewById<EditText>(Resource.Id.editTextNomeUser);
            var emailUser = FindViewById<EditText>(Resource.Id.editTextEmailUser);
            var senhaUser = FindViewById<EditText>(Resource.Id.editTextPassUser);
            var confSenhaUser = FindViewById<EditText>(Resource.Id.editTextConfirmPassUser);

            if (senhaUser?.Text == confSenhaUser?.Text)
            {
                // Crie um objeto com os dados que deseja salvar
                var dados = new
                {
                    Nome = nomeUser?.Text,
                    Senha = senhaUser?.Text,
                    Email = emailUser?.Text
                };
                try
                {
                    string jsonDados = JsonConvert.SerializeObject(dados);

                    FirebaseClient firebase = new FirebaseClient("https://ifpr-alerts-default-rtdb.firebaseio.com/");
                    var result = await firebase
                        .Child("usuarios")
                        .PostAsync(jsonDados);

                    if (result != null)
                    {
                        // reinicia valores dos campos da tela
                        nomeUser.Text = "";
                        senhaUser.Text = "";
                        emailUser.Text = "";
                        confSenhaUser.Text = "";

                        Toast.MakeText(this, "Cadastro realizado com sucesso!", ToastLength.Short)?.Show();
                    }
                    else
                    {
                        Toast.MakeText(this, "O cadastro não pôde ser concluído!", ToastLength.Short)?.Show();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                }
            }
            else
            {
                Toast.MakeText(this, "As senha estão diferentes!", ToastLength.Short)?.Show();
            }

        }

    }
}
