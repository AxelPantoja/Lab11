using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;

namespace Lab11
{
    [Activity(Label = "Lab11", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Complex Data;
        private ValidationStatus Validacion;
        private int Counter = 0;
        private TextView tvResultado;

        protected async override void OnCreate(Bundle bundle)
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnCreate");

            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            tvResultado = FindViewById<TextView>(Resource.Id.tvResultado);

            FindViewById<Button>(Resource.Id.StartActivity).Click += (sender, e) =>
            {
                var ActivityIntent = new Intent(this, typeof(SecondActivity));

                StartActivity(ActivityIntent);
            };

            //FragmentManager para recuperar el fragmento:
            Data = (Complex) this.FragmentManager.FindFragmentByTag("Data");
            if (Data == null)
            {
                //No ha sido almacenado, agregar el fragmento a la activity:
                Data = new Complex();
                var FragmentTransaction = this.FragmentManager.BeginTransaction();

                FragmentTransaction.Add(Data, "Data");
                FragmentTransaction.Commit();
            }

            if (bundle != null)
            {
                Counter = bundle.GetInt("CounterValue", 0);
                Android.Util.Log.Debug("Lab11Log", "Activity A - Recovered Instace State");
            }

            var ClickCounter = FindViewById<Button>(Resource.Id.ClicksCounter);

            ClickCounter.Text = Resources.GetString(Resource.String.ClicksCounter_Text,
                Counter);

            ClickCounter.Text += $"\n{Data.ToString()}";

            ClickCounter.Click += (sender, e) =>
            {
                Counter++;
                ClickCounter.Text =
                    Resources.GetString(Resource.String.ClicksCounter_Text, Counter);

                //Modificar con cualquier vsalor para verificar la persistencia:
                Data.Imaginary++;
                Data.Real++;
                //Mostrar el valor de los miembros:
                ClickCounter.Text += $"\n{Data.ToString()}";
            };

            //Codigo para la validacion:
            //Recuperamos el fragment:
            Validacion = (ValidationStatus) this.FragmentManager.FindFragmentByTag("Validacion");
            if (Validacion == null)
            {
                Validacion = new ValidationStatus();
                var Transaction = this.FragmentManager.BeginTransaction();
                Transaction.Add(Validacion, "Validacion");
                Transaction.Commit();
            }

            if (!Validacion.YaValidado)
            {
                Validacion.Resultado = await Validar();
                Validacion.YaValidado = true;
                tvResultado.Text = Validacion.Resultado;
            }
            else
            {
                tvResultado.Text = Validacion.Resultado;
            }
        }

        private async Task<string> Validar()
        {
            string device = Android.Provider.Settings.Secure.GetString(
                ContentResolver, Android.Provider.Settings.Secure.AndroidId);
            string email = "";
            string password = "";

            var client = new SALLab11.ServiceClient();
            var respuesta = await client.ValidateAsync(email, password, device);

            string resultado = $"{respuesta.Status}\n{respuesta.Fullname}\n{respuesta.Token}";
            return resultado;
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("CounterValue", Counter);
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnSaveInstanceState");

            base.OnSaveInstanceState(outState);
        }

        protected override void OnStart()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnStart");
            base.OnStart();
        }

        protected override void OnResume()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnResume");
            base.OnResume();
        }

        protected override void OnPause()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnPause");
            base.OnPause();
        }

        protected override void OnStop()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnStop");
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnDestroy");
            base.OnDestroy();
        }

        protected override void OnRestart()
        {
            Android.Util.Log.Debug("Lab11Log", "Activity A - OnRestart");
            base.OnRestart();
        }
    }
}