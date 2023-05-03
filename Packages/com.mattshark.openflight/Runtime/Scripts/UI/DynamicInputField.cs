using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using TMPro;

namespace OpenFlightVRC
{
	public class DynamicInputField : UdonSharpBehaviour
	{
		public UdonBehaviour target;
		public int decimalPlaces = 2;
		public string targetVariable;
		public string prefix = "";
		public string suffix = "";
		InputField field;
		bool isStringType = false;
		bool isBoolType = false;

		void Start()
		{
			//get the real target from the proxy, if it exists
			if (target.GetProgramVariable("target") != null)
				target = (UdonBehaviour)target.GetProgramVariable("target");

			//text = GetComponent<TextMeshProUGUI>();
			field = GetComponent<InputField>();
			//determine if the target variable is a string
			var targetType = target.GetProgramVariableType(targetVariable);
			if (targetType == typeof(string))
			{
				isStringType = true;
			}
			//determine if the target variable is a bool
			if (targetType == typeof(bool))
			{
				isBoolType = true;
			}
		}

		void Update()
		{
			var targetValue = target.GetProgramVariable(targetVariable);
			//determine if it is a bool
			if (isBoolType)
			{
				if ((bool)targetValue)
				{
					field.text = prefix + "True" + suffix;
				}
				else
				{
					field.text = prefix + "False" + suffix;
				}
			}
			else if (!isStringType)
			{
				float roundingModifier = Mathf.Pow(10, decimalPlaces);
				float rounded = Mathf.Round((float)targetValue * roundingModifier) / roundingModifier;
				field.text = prefix + rounded.ToString() + suffix;
			}
			else
			{
				field.text = prefix + targetValue.ToString() + suffix;
			}
		}
	}
}
