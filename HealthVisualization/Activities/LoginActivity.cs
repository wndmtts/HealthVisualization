using Firebase.Database;
using HealthVisualization.BaseClasses;

namespace HealthVisualization.Activities
{

    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_login);


            Button loginButton = FindViewById<Button>(Resource.Id.buttonLogin);

            // sobrecarrega o método de Click
            loginButton.Click += LoginButton_Click;
        }

        private async void LoginButton_Click(object? sender, EventArgs e)
        {
            // captura os valores do campos de texto da tela
            var email = FindViewById<EditText>(Resource.Id.editTextEmail)?.Text;
            var password = FindViewById<EditText>(Resource.Id.editTextPassword)?.Text;

            //Conecta com o banco de dados Realitme Database do Firebase
            FirebaseClient firebase = new FirebaseClient("https://ifpr-alerts-default-rtdb.firebaseio.com/");

            var usuario = (await firebase
                .Child("usuarios")
                .OnceAsync<Usuario>()).Select(item => new Usuario
                {
                    Email = item.Object.Email,
                    Senha = item.Object.Senha,
                    Nome = item.Object.Nome
                }).Where(item => item.Email == email).FirstOrDefault();

            if (usuario != null)
            {
                if(usuario.Senha == password)
                {
                    Toast.MakeText(this, "Usuário logado com sucesso!", ToastLength.Short)?.Show();
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Senha incorreta. Digite novamente!", ToastLength.Short)?.Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Usuário não encontrado!", ToastLength.Short)?.Show();
            }
        }
        
    }
}