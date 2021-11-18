using System;
using System.Globalization;
using System.Numerics;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.Icu.Math;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Java.Lang;
using static System.Console;
using Double = System.Double;
using Math = System.Math;
using String = System.String;

namespace Calculator
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true,ScreenOrientation= ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        private bool _isFloat = false;
        private int _lastDigit;
        System.Double _number1;
        System.Double _number2;
        private Double _numResult;
        /// <summary>
        ///  string operations
        /// </summary>
        private bool so = true;
        // private System.Double _numView;
        Stage _stage;
        private Operation _operation;

        private int step;


        private enum Stage
        {
            Number1,
            Number2,
            Operator,
            Multi,
            Result
        }


        enum Operation
        {
            Add,
            Subtract,
            Multiply,
            Division,
            SqRoot,
            Undefined
        }

        struct Opr
        {
            internal double Number;
            internal Operation Operation;
            internal Stage Stage;
            internal double Result;
        }

        /// <summary>
        /// Operations
        /// </summary>
        //  private Opr[] _oprs;
      

        private string _operSymbol = String.Empty;
        private TextView _textView;
        private TextView _textPreview;
        BigInteger biginteger;
        BigDecimal bd;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            

            Button button0 = FindViewById<Button>(Resource.Id.button_0);
            Button button1 = FindViewById<Button>(Resource.Id.button_1);
            Button button2 = FindViewById<Button>(Resource.Id.button_2);
            Button button3 = FindViewById<Button>(Resource.Id.button_3);
            Button button4 = FindViewById<Button>(Resource.Id.button_4);
            Button button5 = FindViewById<Button>(Resource.Id.button_5);
            Button button6 = FindViewById<Button>(Resource.Id.button_6);
            Button button7 = FindViewById<Button>(Resource.Id.button_7);
            Button button8 = FindViewById<Button>(Resource.Id.button_8);
            Button button9 = FindViewById<Button>(Resource.Id.button_9);

            _textView = FindViewById<TextView>(Resource.Id.textView1);
           
            _textPreview = FindViewById<TextView>(Resource.Id.textViewPreview);
            _textView.Text = _numResult.ToString(format: "N0");
            _textPreview.Text = String.Empty;
            button0.Click += new EventHandler(BtnDigit_Clicked);
            button1.Click += new EventHandler(BtnDigit_Clicked);
            button2.Click += new EventHandler(BtnDigit_Clicked);
            button3.Click += new EventHandler(BtnDigit_Clicked);
            button4.Click += new EventHandler(BtnDigit_Clicked);
            button5.Click += BtnDigit_Clicked;
            button6.Click += BtnDigit_Clicked;
            button7.Click += new EventHandler(BtnDigit_Clicked);
            button8.Click += new EventHandler(BtnDigit_Clicked);
            button9.Click += new EventHandler(BtnDigit_Clicked);

            Button buttonCe = FindViewById<Button>(Resource.Id.button_ce);
            buttonCe.Click += new EventHandler(BtnCe_Clicked);

            Button buttonEq = FindViewById<Button>(Resource.Id.button_equals);
            buttonEq.Click += new EventHandler(BtnEq_Clicked);

            Button buttonBack = FindViewById<Button>(Resource.Id.button_back);
            buttonBack.Click += new EventHandler(BtnBack_Clicked);

            Button buttonAdd = FindViewById<Button>(Resource.Id.button_add);
            buttonAdd.Click += new EventHandler(BtnAdd_Clicked);

            Button buttonMul = FindViewById<Button>(Resource.Id.button_multi);
            buttonMul.Click += new EventHandler(BtnMul_Clicked);

            Button buttonDiv = FindViewById<Button>(Resource.Id.button_div);
            buttonDiv.Click += new EventHandler(BtnDiv_Clicked);

            Button buttonSub = FindViewById<Button>(Resource.Id.button_minus);
            buttonSub.Click += new EventHandler(BtnSub_Clicked);

            Button buttonSqrt = FindViewById<Button>(Resource.Id.button_sqrt);
            buttonSqrt.Click += new EventHandler(BtnSqrt_Clicked);

            Button buttondot = FindViewById<Button>(Resource.Id.button_dot);
            buttondot.Click += BtnDot_Clicked;

            Button buttonPlusMinus = FindViewById<Button>(Resource.Id.button_plus_minus);
            buttonPlusMinus.Click += BtnPlusMinus_Clicked;

            _number1 = 0;
            _number2 = 0;
            _numResult = 0;

            /* _oprs = new Opr[1];
             Array.Resize(ref _oprs, 3);
             */
           
        }

      

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void BtnAdd_Clicked(object sender, EventArgs e)
        {
            switch (_stage)
            {
                case Stage.Number1:
                    _operation = Operation.Add;
                    _isFloat = false;
                    _stage = Stage.Operator;
                    break;
                case Stage.Number2:

                    break;
            }

            _operSymbol = "+";

            Toast.MakeText(Android.App.Application.Context, "+", ToastLength.Short).Show();
            Calculate();
        }

        private void BtnSub_Clicked(object sender, EventArgs e)
        {
            switch (_stage)
            {
                case Stage.Number1:
                    _operation = Operation.Subtract;
                    _isFloat = false;
                    _stage = Stage.Operator;
                    break;
                case Stage.Number2:
                    _number1 -= _number2; // İki sayıyı toplayıp 3. sayı için yeni toplama olarak devam et
                    _numResult = 0;
                    _stage = Stage.Number1;
                    break;
            }

            _operSymbol = "-";
            Toast.MakeText(Android.App.Application.Context, "-", ToastLength.Short).Show();
            Calculate();
        }

        private void BtnMul_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (_stage)
            {
                case Stage.Number1:
                    _operation = Operation.Multiply;
                    _isFloat = false;
                    _stage = Stage.Operator;
                    break;
                case Stage.Number2:
                    _number1 *= _number2; // İki sayıyı toplayıp 3. sayı için yeni toplama olarak devam et
                    _numResult = 0;
                    _stage = Stage.Number1;
                    break;
            }

            _operSymbol = "x";
            Toast.MakeText(Android.App.Application.Context, "x", ToastLength.Short).Show();
            Calculate();
        }

        public void BtnDiv_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (_stage)
            {
                case Stage.Number1:
                    _operation = Operation.Division;
                    _isFloat = false;
                    _stage = Stage.Operator;
                    break;
                case Stage.Number2:
                    _number1 /= _number2; // İki sayıyı toplayıp 3. sayı için yeni toplama olarak devam et
                    _numResult = 0;
                    _stage = Stage.Number1;
                    break;
            }

            _operSymbol = "/";
            Toast.MakeText(Android.App.Application.Context, "/", ToastLength.Short).Show();
            Calculate();
        }

        public void BtnSqrt_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
#if SO

#else
            switch (_stage)
            {
                case Stage.Number1:
                    _operation = Operation.Undefined;
                    _isFloat = false;
                    _numResult = Math.Sqrt(_number1);
                    _stage = Stage.Result;
                    break;
                case Stage.Number2:
                    break;
            }

            Toast.MakeText(Android.App.Application.Context, "/", ToastLength.Short).Show();
            Calculate();
#endif
        }

        public void BtnMemPlus_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
        }

        public void BtnMemCall_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
        }

        public void BtnMemReset_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
        }

        private void BtnDot_Clicked(object sender, EventArgs e)
        {
            _isFloat = true;
#if SO

#else
            dblMtp = 1;
#endif
        }

        int intMtp = 10; // Tamsayı kısmı çarpanı
        private Double dblMtp = 1; // Kesirli sayı çarpanı



        public void BtnDigit_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            _lastDigit = int.Parse(button.Text);
#if SO      //String Operations
            _textView.Text += button.Text;

            WriteLine("BtnDigit " + _stage);
            switch (_stage)
            {
                case Stage.Number1:
                    _number1 = double.Parse(_textView.Text);

                    break;
                case Stage.Number2:
                    break;
                case Stage.Operator:
                    break;
                case Stage.Result:
                    break;
                default:
                    break;
            }
#else
            if (_isFloat)
            {
                intMtp = 1;
                dblMtp *= 0.1;
            }
            else
            {
                intMtp = 10;
                dblMtp = 1;
            }

            WriteLine("BtnDigit " + _stage);
            switch (_stage)
            {
                case Stage.Number1:
                    _number1 = (_number1 * intMtp) + _lastDigit * dblMtp;
                    Calculate();
                    break;
                case Stage.Number2:
                    _number2 = (_number2 * intMtp) + _lastDigit * dblMtp;
                    Calculate();
                    break;
                case Stage.Operator:
                    _number2 = (_number2 * intMtp) + _lastDigit * dblMtp;
                    Calculate();
                    //Buraya kadar yapılan herşeyi Number2 ye de yaz
                    _stage = Stage.Number2;
                    break;
                case Stage.Multi:
                    break;
                case Stage.Result:
                    Calculate();
                    BtnCe_Clicked(sender, e);
                    _stage = Stage.Number1;
                    _number1 = (_number1 * intMtp) + _lastDigit * dblMtp;

                    break;
            }
#endif
        }

        private void BtnPlusMinus_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            switch (_stage)
            {
                case Stage.Number1:
                    _number1 *= -1;

                    break;
                case Stage.Number2:
                    _number2 *= -1;
                    break;
                case Stage.Result:
                    _numResult *= -1;
                    break;
            }
            Render();
        }

        public void BtnCe_Clicked(object sender, EventArgs e)
        {
            //todo : Burada eski sonucu geçmişe taşı
            Button button = (Button)sender;
            System.Threading.Thread.Sleep(50);
            _number1 = 0;
            _number2 = 0;
            _numResult = 0;
            _isFloat = false;
            _stage = Stage.Number1;
            Calculate();
        }

        private void BtnEq_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (_stage == Stage.Number2)
            {

                _stage = Stage.Result;
                Calculate();
                _lastDigit = 0;
                _number1 = 0;
                _number2 = 0;
                _isFloat = false;
            }
        }

       

        private void Calculate()
        {
            switch (_operation)
            {
                case Operation.Add:
                    {
                        _numResult = _number1 + _number2;
                        break;
                    }
                case Operation.Subtract:
                    {
                        _numResult = _number1 - _number2;
                        break;
                    }
                case Operation.Multiply:
                    _numResult = _number1 * _number2;
                    break;
                case Operation.Division:
                    {
                        _numResult = _number1 / _number2;
                        break;
                    }

            }
            Render();
        }


        private void Render()
        {

            switch (_stage)
            {
                case Stage.Number1:
                    _textView.Text = _number1.ToString();
                    break;
                case Stage.Number2:
                    _textView.Text = _number2.ToString();
                    _textPreview.Text = _number1 + _operSymbol + _number2 + "=" + _numResult;
                    break;
                case Stage.Operator:
                    _textPreview.Text = _number1 + _operSymbol + _number2 + "=" + _numResult;
                    break;
                case Stage.Result:
                    _textView.Text = _numResult.ToString();
                    _textPreview.Text = String.Empty;
                    break;

                default:
                    break;
            }
        }
        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
           
                switch (_stage)
                {
                    case Stage.Number1:
                        if (_isFloat)
                        {
                            _number1 /= dblMtp;
                            _number1 = Math.Floor(_number1 / 10);
                            dblMtp *= 10;
                            _number1 *= dblMtp;
                            if (dblMtp >= 1)
                            {
                                _isFloat = false;
                            }
                        }
                        else
                        {
                            _number1 = Math.Floor(_number1 / 10);
                        }

                        Calculate();
                        break;
                }
                _numResult = Math.Floor(_numResult/10);
                _numResult = _numResult - _lastDigit;
           

            _lastDigit = 0;
            Calculate();
            //todo : - işaretinli bir sayıda back'e basılırsa sorun çıkabilr
        }

        private int[] decimals = new int[20];
        private int[] points = new int[20];
        void seperate()
        {
            Double r = _numResult;



        }

        private bool IsDouble(double doubleNum)
        {
            string strResult;
            double dblResult = doubleNum;
            int intResult = (int)dblResult;
            if (Math.Abs(intResult - dblResult) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}