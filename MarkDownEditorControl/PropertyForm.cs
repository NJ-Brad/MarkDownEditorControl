using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace MarkDownHelper
{
    public partial class PropertyForm : Form
    {
        public PropertyForm()
        {
            InitializeComponent();
        }
        public ChangeDelegate ProcessChange { get; set; }

        public DialogResult ShowDialog(object dataObject)
        {
            this.DataObject = dataObject;

            return base.ShowDialog();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            Type objType = DataObject.GetType();
            PropertyInfo[] pi = objType.GetProperties();

            ClearProperties();

            foreach (PropertyInfo pinf in pi)
            {
                MarkDownItemPropertyAttribute interestingInfo = pinf.GetCustomAttribute<MarkDownItemPropertyAttribute>();
                if (interestingInfo != null)
                {
                    AddProperty(pinf.Name, interestingInfo.Prompt, interestingInfo.PropertyType, interestingInfo.Choices);
                }
            }
        }

        private void Changed(string fieldName, string fieldValue)
        {
            if (ProcessChange != null)
            {
                ChangeDetails details = new ChangeDetails { FieldName = fieldName, FieldValue = fieldValue };

                ProcessChange(details);
            }
        }

        public void ClearProperties()
        {
            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if (!ctrl.IsDisposed)
                {
                    ctrl.Dispose();
                }
            }

            tableLayoutPanel1.Controls.Clear();
            tableLayoutPanel1.RowCount = 0;
            tableLayoutPanel1.RowStyles.Clear();
        }

        public void AddProperty(string propertyName, string prompt, string propertyType, params string[] choices)
        {
            Dictionary<string, string> choiceDict = new Dictionary<string, string>();
            foreach (string str in choices)
            {
                choiceDict.Add(str, str);
            }

            AddProperty(propertyName, prompt, propertyType, choiceDict);
        }


        //public void AddProperty(string propertyName, string prompt, string propertyType, params string[] choices)
        public void AddProperty(string propertyName, string prompt, string propertyType, Dictionary<string, string> choices)
        {
            int newRowNum = tableLayoutPanel1.RowCount;
            int promptWidth = (Convert.ToInt32(tableLayoutPanel1.ColumnStyles[1].Width) * tableLayoutPanel1.Width) / 100;
            int propertyWidth = (Convert.ToInt32(tableLayoutPanel1.ColumnStyles[1].Width) * tableLayoutPanel1.Width) / 100;

            // add a new row
            tableLayoutPanel1.RowCount += 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Controls.Add(new Label { Text = prompt, TextAlign = ContentAlignment.MiddleRight, Width = promptWidth, Height = 30 }, 0, newRowNum);

            Control propertyControl = null;

            switch (propertyType)
            {
                case "Text":
                    propertyControl = new TextBox { Width = propertyWidth, Height = 30, Tag = propertyName };
                    propertyControl.Visible = true;
                    //((TextBox)propertyControl).TextChanged += (s, e) => { SendChangeNotification(s); };
                    ((TextBox)propertyControl).LostFocus += (s, e) => { SendChangeNotification(s); };
                    break;
                case "MultiLineText":
                    propertyControl = new TextBox { Width = propertyWidth, Height = 160, Tag = propertyName, Multiline = true, ScrollBars = ScrollBars.Both, WordWrap = false };
                    //((TextBox)propertyControl).TextChanged += (s, e) => { SendChangeNotification(s); };
                    ((TextBox)propertyControl).LostFocus += (s, e) => { SendChangeNotification(s); };
                    tableLayoutPanel1.RowStyles[tableLayoutPanel1.RowStyles.Count - 1] = (new RowStyle(SizeType.Absolute, 160F));
                    break;
                case "Choice":
                    propertyControl = new ComboBox { Width = propertyWidth, Height = 30, DropDownStyle = ComboBoxStyle.DropDownList, Tag = propertyName };
                    ((ComboBox)propertyControl).SelectedIndexChanged += (s, e) => { SendChangeNotification(s); };

                    ((ComboBox)propertyControl).ValueMember = "Key";
                    ((ComboBox)propertyControl).DisplayMember = "Value";
                    ((ComboBox)propertyControl).DataSource = choices.ToList<KeyValuePair<string, string>>();

                    //foreach (string choice in choices)
                    //{
                    //    ((ComboBox)propertyControl).Items.Add(choice);
                    //}
                    break;
                case "Flag":
                    //propertyControl = new TextBox { Width = propertyWidth, Height = 30, Tag = propertyName };
                    //((TextBox)propertyControl).TextChanged += PropertyTable_TextChanged;
                    propertyControl = new CheckBox { Height = 30, Tag = propertyName };
                    ((CheckBox)propertyControl).CheckedChanged += (s, e) => { SendChangeNotification(s); };
                    break;
            }
            if (propertyControl != null)
            {
                tableLayoutPanel1.Controls.Add(propertyControl, 1, newRowNum);
            }
        }

        private void SendChangeNotification(object sender)
        {
            if (!settingValueProgramatically)
            {
                if (((Control)sender).Tag != null)
                {
                    string fieldName = ((Control)sender).Tag.ToString();
                    string fieldValue = GetCtrlValue((Control)sender);

                    Changed(fieldName, fieldValue);
                }
            }
        }

        public string GetValue(string propertyName)
        {
            string rtnVal = "";

            Control ctrl = FindControlHoldingPropertyValue(propertyName);
            if (ctrl != null)
            {
                rtnVal = GetCtrlValue(ctrl);
            }

            return rtnVal;
        }

        bool settingValueProgramatically = false;

        public void SetValue(string propertyName, string value)
        {
            settingValueProgramatically = true;
            Control ctrl = FindControlHoldingPropertyValue(propertyName);
            if (ctrl != null)
            {
                SetCtrlValue(ctrl, value);
            }
            settingValueProgramatically = false;
        }

        private static void SetCtrlValue(Control ctrl, string value)
        {
            Type type = ctrl.GetType();
            switch (type.Name)
            {
                case "TextBox":
                    ((TextBox)ctrl).Text = value;
                    break;
                case "ComboBox":
                    // ((ComboBox)ctrl).SelectedIndex = ((ComboBox)ctrl).FindStringExact(value);
                    if (value == null)
                    {
                        ((ComboBox)ctrl).SelectedIndex = -1;
                    }
                    else
                    {
                        ((ComboBox)ctrl).SelectedValue = value;
                    }
                    break;
                case "CheckBox":
                    ((CheckBox)ctrl).Checked = value == "Y";
                    break;
            }
        }

        private static string GetCtrlValue(Control ctrl)
        {
            string rtnVal = "";

            Type type = ctrl.GetType();
            switch (type.Name)
            {
                case "TextBox":
                    rtnVal = ((TextBox)ctrl).Text;
                    break;
                case "ComboBox":
                    if (((ComboBox)ctrl).SelectedItem != null)
                    {
                        //rtnVal = ((ComboBox)ctrl).SelectedItem.ToString();
                        rtnVal = ((ComboBox)ctrl).SelectedValue.ToString();
                    }
                    break;
                case "CheckBox":
                    rtnVal = ((CheckBox)ctrl).Checked ? "Y" : "N";
                    break;
            }
            return rtnVal;
        }

        private Control FindControlHoldingPropertyValue(string propertyName)
        {
            Control rtnVal = null;

            foreach (Control ctrl in tableLayoutPanel1.Controls)
            {
                if ((ctrl.Tag != null) && (ctrl.Tag.ToString() == propertyName))
                {
                    rtnVal = ctrl;
                    break;
                }
            }

            return rtnVal;
        }

        public delegate void ChangeDelegate(ChangeDetails d);

        public object DataObject { get; set; } = null;

        private void OkClick(object sender, EventArgs e)
        {
            Type objType = DataObject.GetType();
            PropertyInfo[] pi = objType.GetProperties();

            foreach (PropertyInfo pinf in pi)
            {
                pinf.SetValue(DataObject, GetValue(pinf.Name));
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}