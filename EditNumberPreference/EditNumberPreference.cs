using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Messert.Controls.Droid
{
    [Register("com.messert.preferences.droid.EditNumberPreference")]
    public class EditNumberPreference : EditTextPreference
    {
		public int MinValue { get; set; }
        public int MaxValue { get; set; }

        public event EventHandler<InvalidInputEventArgs> InvalidInput;

		private int _initialValue;
		
        public EditNumberPreference(Context context) : base(context) { }
        public EditNumberPreference(Context context, IAttributeSet attrs) : base(context, attrs) { InitializeFromAttributes(context, attrs); }
        public EditNumberPreference(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle) { InitializeFromAttributes(context, attrs); }

        protected EditNumberPreference(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

        private void InitializeFromAttributes(Context context, IAttributeSet attrs)
        {
            var typedArray = context.ObtainStyledAttributes(attrs, Resource.Styleable.EditNumberPreference, 0, 0);

            MinValue = typedArray.GetInteger(Resource.Styleable.EditNumberPreference_minValue, 0);
            MaxValue = typedArray.GetInteger(Resource.Styleable.EditNumberPreference_maxValue, int.MaxValue);
        }

        protected override void OnAddEditTextToDialogView(View dialogView, EditText editText)
        {
            editText.InputType = Android.Text.InputTypes.ClassNumber|Android.Text.InputTypes.NumberVariationNormal;
            editText.Text = _initialValue.ToString();

            base.OnAddEditTextToDialogView(dialogView, editText);
        }

        public override void OnClick(IDialogInterface dialog, int which)
        {
            switch ((DialogButtonType)which)
            {
                case DialogButtonType.Positive:
                    if (!int.TryParse(EditText.Text, out int result))
                    {
                        InvalidInput?.Invoke(this, new InvalidInputEventArgs("Please enter a valid number"));
                        EditText.ClearFocus();
                    }
                    else if (result < MinValue)
                    {
                        InvalidInput?.Invoke(this, new InvalidInputEventArgs("Value must be at least " + MinValue));
                        EditText.ClearFocus();
                    }
                    else if (result > MaxValue)
                    {
                        InvalidInput?.Invoke(this, new InvalidInputEventArgs("Value must be at most " + MaxValue));
                        EditText.ClearFocus();
                    }
                    else
                    {
                        base.OnClick(dialog, which);
                    }
                    break;
            }
        }

        protected override void OnDialogClosed(bool positiveResult)
        {
            if (positiveResult)
            {
                EditText.ClearFocus();
                int value = int.Parse(EditText.Text);
                SetValue(value);
            }
        }

        protected override void OnSetInitialValue(bool restorePersistedValue, Java.Lang.Object defaultValue)
        {
            int valueInt = (int)defaultValue;
            SetValue(restorePersistedValue ? GetPersistedInt(valueInt) : valueInt);
        }

        private void SetValue(int value)
        {
            if (ShouldPersist())
            {
                PersistInt(value);
            }

            if (_initialValue != value)
            {
                _initialValue = value;
                NotifyChanged();
            }
        }

        protected override Java.Lang.Object OnGetDefaultValue(TypedArray a, int index)
        {
            _initialValue = a.GetInt(index, 1);
            return _initialValue;
        }
    }
}
