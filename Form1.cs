using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
//see 
using System.IO;
using System.Linq;
using System.Windows.Forms;



/* day1 -. name and mobile 
 *  default 
 *  name 
 *  mobile 
 *  paper
 *  reports 
 *  
 *  
 *  day2 -gui(current )
 *  
 *  
 *  default data 
 *  hardcode
 *  
 *  name ;
 *  mobile = "customer_no"
 *  paper = false
 *  
 *  
 *
 *  paper_check 
 * 
 * 
 * gui ->(current) 
 * paper check 
 * 
 * default_data : 
 * name -> true or false
 * mobile -> 
 * 
 * 
 * 
 */
namespace test_new_popup
{
    public partial class Form1 : Form
    {
        private readonly List<(string phone, string name)> contacts = new List<(string, string)>();
        private ListBox suggestionBoxMobile;
        private ListBox suggestionBoxName;
        private bool _updatingFields = false;

        // Current transaction fields captured at click time
        private string currentMobile = "0000000000";
        private string currentName = "";
        private bool currentPaperState = false;
        private bool showGender = false;
        private string selectedGender = "";
        private bool showUnder18 = false;
        private bool isUnder18 = false;

        private const string ResJsonPath = @"C:\BillBox\common\configs\res.json";
        private const string DataJsonPath = @"C:\BillBox\common\configs\data.json";

        private const int WM_SYSCOMMAND = 0x0112;
        private const int WM_KEYDOWN = 0x0100;
        private const int SC_MINIMIZE = 0xF020;
        private const int SC_CLOSE = 0xF060;
        private const int VK_ESCAPE = 0x1B;

        private bool isSubmit = false;

        private static readonly Font Line1Font = new Font("Segoe UI Semibold", 14);
        private static readonly Font Line2Font = new Font("Segoe UI", 11);

        public Form1()
        {
            this.StartPosition = FormStartPosition.Manual;
            CenterToScreen();
            InitializeComponent();
            InitializeSuggestionBoxes();
            GetValidatedData();

            this.MaximizeBox = false;
            this.ControlBox = true;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.TopMost = true;

            this.Load += Form1_Load;
            this.FormClosing += Form1_FormClosing;

            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;

            this.Resize += (s, e) =>
            {
                if (this.WindowState == FormWindowState.Minimized)
                    this.WindowState = FormWindowState.Normal;
            };

            button1.Click += (s, e) =>
            {
                isSubmit = true;   // mark submit

                currentMobile = boxMobile.Text.Trim();
                currentName = boxName.Text.Trim();
                currentPaperState = checkBox1.Checked;

                // ✅ CASE 1: BOTH EMPTY
                if (string.IsNullOrWhiteSpace(currentMobile) &&
                    string.IsNullOrWhiteSpace(currentName))
                {
                    // ✅ Paper checkbox mandatory
                    if (!checkBox1.Checked)
                    {
                        MessageBox.Show("Select Paper-Bill or enter mobile",
                            "Validation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        checkBox1.Focus();
                        return;
                    }

                    // ✅ allow if paper checked
                }
                else
                {
                    // ✅ CASE 2: MOBILE VALIDATION (if entered)
                    if (!string.IsNullOrWhiteSpace(currentMobile))
                    {
                        if (currentMobile.Length != 10 || !currentMobile.All(char.IsDigit))
                        {
                            MessageBox.Show("Enter valid mobile number",
                                "Invalid Mobile",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);

                            boxMobile.Focus();
                            return;
                        }
                    }
                    else
                    {
                        // ✅ CASE 3: NAME only (no mobile) → NOT allowed for E‑Bill
                        MessageBox.Show("Mobile number required for E‑Bill",
                            "Validation",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        boxMobile.Focus();
                        return;
                    }
                }

                // NAME DUPLICATE CHECK (only if name entered)
                if (!string.IsNullOrWhiteSpace(currentName))
                {
                    if (contacts.Any(c =>
                        c.name.Equals(currentName, StringComparison.OrdinalIgnoreCase) &&
                        c.phone != currentMobile))
                    {
                        MessageBox.Show("Name already used for another number",
                            "Warning",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        boxName.Focus();
                        return;
                    }
                }

                // ONLY SET AFTER SUCCESS
                isSubmit = true;

                // Only add if name is NOT empty
                if (!string.IsNullOrWhiteSpace(currentName))
                {
                    if (!contacts.Any(c => c.phone == currentMobile))
                    {
                        contacts.Add((currentMobile, currentName));
                    }
                }
                string paper = "0";
                if (currentPaperState) paper = "1";
                else paper = "0";
                string args = $"{currentMobile} \"{currentName}\" {paper} ";
                this.Close();
            };
        }

        private JObject GetValidatedData()
        {
            string defaultPath = @"C:\BillBox\common\configs\default_data.json";

            JObject template = new JObject
            {
                ["name"] = "",
                ["mobile"] = "0000000000",
                ["paper"] = false,
                ["under18"] = false,
                ["gender"] = "",
                ["reports"] = false,
                ["is_submit"] = false
            };

            try
            {
                if (!File.Exists(defaultPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(defaultPath));
                    File.WriteAllText(defaultPath, template.ToString(Newtonsoft.Json.Formatting.Indented));
                    return template;
                }

                JObject current = JObject.Parse(File.ReadAllText(defaultPath));

                foreach (var prop in template.Properties())
                {
                    string key = prop.Name;
                    JToken def = prop.Value;
                    JToken cur = current[key];

                    if (cur == null)
                    {
                        current[key] = def;
                        continue;
                    }

                    switch (key)
                    {
                        case "name":
                            if (cur.Type != JTokenType.String)
                                current[key] = "";
                            break;

                        case "mobile":
                            string mobile = cur.ToString();

                            // ✅ ALLOW BOTH VALUES
                            if (mobile != "0000000000" &&
                                mobile != "customer_no" &&
                                (mobile.Length != 10 || !mobile.All(char.IsDigit)))
                            {
                                current[key] = "0000000000";
                            }
                            break;

                        case "paper":
                        case "under18":
                        case "reports":
                        case "is_submit":
                            if (cur.Type != JTokenType.Boolean)
                                current[key] = def;
                            break;

                        case "gender":
                            string g = cur.ToString().ToLower();
                            if (g != "" && g != "male" && g != "female")
                                current[key] = "";
                            break;
                    }
                }

                // ✅ ALWAYS rewrite
                File.WriteAllText(defaultPath, current.ToString(Newtonsoft.Json.Formatting.Indented));

                return current;
            }
            catch
            {
                File.WriteAllText(defaultPath, template.ToString(Newtonsoft.Json.Formatting.Indented));
                return template;
            }
        }




        private void WriteMergedDataJson()
        {
            string defaultPath = @"C:\BillBox\common\configs\default_data.json";

            JObject data = GetValidatedData();


            // ✅ Common fields
            if (!string.IsNullOrWhiteSpace(currentName))
                data["name"] = currentName;

            if (!string.IsNullOrWhiteSpace(currentMobile) &&
     currentMobile != "0000000000")
            {
                data["mobile"] = currentMobile;
            }


            data["paper"] = currentPaperState;

            // ✅ Only this popup fields
            if (showGender)
                data["gender"] = selectedGender;

            if (showUnder18)
                data["under18"] = isUnder18;

            data["is_submit"] = true;

            // ❌ DO NOT TOUCH reports

            SafeWriteFile(DataJsonPath, data.ToString(Formatting.Indented));
        }

        private void SafeWriteFile(string path, string content)
        {
            string tempPath = path + ".tmp";

            File.WriteAllText(tempPath, content);

            File.Copy(tempPath, path, true);

            File.Delete(tempPath);
        }

        private void LoadAppSettings()
        {
            try
            {
                string path = @"C:\BillBox\common\configs\popup_config.json";

                if (!File.Exists(path))
                    return;

                string json = File.ReadAllText(path);

                var data = JObject.Parse(json);

                showGender = data["gender_details"]?.Value<bool>() == true;
                showUnder18 = data["under18"]?.Value<bool>() == true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error reading app settings: " + ex.Message);
            }
        }

        private void ApplyGenderSelection(string gender)
        {
            selectedGender = gender;

            // Reset both
            radioMale.BackColor = SystemColors.Control;
            radioMale.ForeColor = Color.Black;

            radioFemale.BackColor = SystemColors.Control;
            radioFemale.ForeColor = Color.Black;

            // Apply selected
            var selected = gender == "Male" ? radioMale : radioFemale;

            selected.BackColor = Color.DodgerBlue;
            selected.ForeColor = Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadContactsFromDisk();
            LoadAppSettings();

            // Visibility
            panelGender.Visible = showGender;
            radioUnder18.Visible = showUnder18;

            // remove default checks if any
            radioMale.Checked = false;
            radioFemale.Checked = false;
            selectedGender = "";

            radioMale.GotFocus += (s, evt) =>
            {
                ApplyGenderSelection("Male");
            };

            radioFemale.GotFocus += (s, evt) =>
            {
                ApplyGenderSelection("Female");
            };

            radioMale.KeyDown += (s, evt) =>
            {
                if (evt.KeyCode == Keys.Right)
                {
                    radioFemale.Focus(); // focus triggers highlight
                }
                else if (evt.KeyCode == Keys.Enter)
                {
                    evt.SuppressKeyPress = true;

                    if (radioUnder18.Visible)
                        radioUnder18.Focus();
                    else
                        button1.Focus();
                }
            };

            radioFemale.KeyDown += (s, evt) =>
            {
                if (evt.KeyCode == Keys.Left)
                {
                    radioMale.Focus(); // highlight auto
                }
                else if (evt.KeyCode == Keys.Enter)
                {
                    evt.SuppressKeyPress = true;

                    if (radioUnder18.Visible)
                        radioUnder18.Focus();
                    else
                        button1.Focus();
                }
            };

            radioMale.GotFocus += (s, evt) =>
            {
                radioMale.FlatAppearance.BorderColor = Color.DodgerBlue;
            };

            radioMale.LostFocus += (s, evt) =>
            {
                radioMale.FlatAppearance.BorderColor = Color.Gray;
            };

            radioFemale.GotFocus += (s, evt) =>
            {
                radioFemale.FlatAppearance.BorderColor = Color.DodgerBlue;
            };

            radioFemale.LostFocus += (s, evt) =>
            {
                radioFemale.FlatAppearance.BorderColor = Color.Gray;
            };

            //radioMale.Checked = true; // default

            // -------------------- UNDER18 TOGGLE --------------------
            radioUnder18.CheckedChanged += (s, evt) =>
            {
                isUnder18 = radioUnder18.Checked;
            };

            // -------------------- NAVIGATION --------------------
            checkBox1.KeyDown += (s, ke) =>
            {
                if (ke.KeyCode == Keys.Enter)
                {
                    ke.SuppressKeyPress = true;

                    if (panelGender.Visible)
                        radioMale.Focus();
                    else if (radioUnder18.Visible)
                        radioUnder18.Focus();
                    else
                        button1.Focus();
                }
            };

            radioUnder18.KeyDown += (s, evt) =>
            {
                if (evt.KeyCode == Keys.Enter)
                {
                    evt.SuppressKeyPress = true;
                    button1.Focus();
                }
            };

            // -------------------- PAPER BILL FOCUS LINE --------------------
            checkBox1.BackColor = SystemColors.Control;

            Panel paperBillLine = new Panel
            {
                Height = 2,
                Width = checkBox1.Width,
                BackColor = Color.DodgerBlue,
                Visible = false
            };

            paperBillLine.Left = checkBox1.Left;
            paperBillLine.Top = checkBox1.Bottom + 2;

            this.Controls.Add(paperBillLine);
            paperBillLine.BringToFront();

            checkBox1.GotFocus += (s, evt) => { paperBillLine.Visible = true; };
            checkBox1.LostFocus += (s, evt) => { paperBillLine.Visible = false; };

            // -------------------- UNDER18 FOCUS LINE --------------------
            Panel under18Line = new Panel
            {
                Height = 2,
                Width = radioUnder18.Width,
                BackColor = Color.DodgerBlue,
                Visible = false
            };

            under18Line.Left = radioUnder18.Left;
            under18Line.Top = radioUnder18.Bottom + 2;

            this.Controls.Add(under18Line);
            under18Line.BringToFront();

            // IMPORTANT: add focus events
            radioUnder18.GotFocus += (s, evt) => { under18Line.Visible = true; };
            radioUnder18.LostFocus += (s, evt) => { under18Line.Visible = false; };

            // -------------------- BUTTON --------------------
            button1.KeyDown += (s, evt) =>
            {
                if (evt.KeyCode == Keys.Enter)
                {
                    evt.SuppressKeyPress = true;
                    button1.PerformClick();
                }
                else if (evt.KeyCode == Keys.Up)
                {
                    evt.SuppressKeyPress = true;
                    checkBox1.Focus();
                }
            };

            // Startup focus
            boxMobile.Focus();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                string defaultPath = @"C:\BillBox\common\configs\default_data.json";

                JObject data = GetValidatedData();


                // ✅ USER CLICKED X
                if (!isSubmit)
                {
                    data["is_submit"] = false;

                    SafeWriteFile(DataJsonPath, data.ToString(Formatting.Indented));
                    return;
                }

                string directory = Path.GetDirectoryName(ResJsonPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // ✅ SAVE res.json → ONLY mobile + name
                var resRoot = new JObject
                {
                    ["records"] = JArray.FromObject(
                        contacts
                            .Where(c => !string.IsNullOrWhiteSpace(c.name)) // filter
                            .Select(c => new JObject
                            {
                                ["customer_phone"] = c.phone,
                                ["customer_name"] = c.name
                            })
                    )
                };

                SafeWriteFile(ResJsonPath, resRoot.ToString(Formatting.Indented));


                // SAVE data.json → FULL DATA (TEMP)
                WriteMergedDataJson();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during shutdown serialization: {ex.Message}");
            }
        }

        private void InitializeSuggestionBoxes()
        {
            suggestionBoxMobile = CreateSuggestionBox();
            suggestionBoxName = CreateSuggestionBox();

            Controls.Add(suggestionBoxMobile);
            Controls.Add(suggestionBoxName);

            suggestionBoxMobile.BringToFront();
            suggestionBoxName.BringToFront();
        }

        private ListBox CreateSuggestionBox()
        {
            var box = new ListBox
            {
                Visible = false,
                DrawMode = DrawMode.OwnerDrawFixed,
                ItemHeight = 40,
                Font = new Font("Segoe UI", 13),
                BackColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            box.DrawItem += SuggestionBox_DrawItem;
            box.Click += (s, e) => SelectSuggestion((ListBox)s);
            box.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SelectSuggestion((ListBox)s);
                    e.Handled = true;
                }
            };

            return box;
        }

        private void LoadContactsFromDisk()
        {
            try
            {
                if (!File.Exists(ResJsonPath)) return;

                string json = File.ReadAllText(ResJsonPath);
                var doc = JObject.Parse(json);
                var records = doc["records"];

                contacts.Clear();
                if (records != null)
                {
                    foreach (var record in records)
                    {
                        string name = record["customer_name"]?.ToString()?.Trim();
                        string phone = record["customer_phone"]?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(name))
                        {
                            contacts.Add((phone, name));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error restoring context records: {ex.Message}");
            }

            boxMobile.TextChanged += BoxMobile_TextChanged;
            boxMobile.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Down && suggestionBoxMobile.Visible)
                {
                    suggestionBoxMobile.Focus();
                    if (suggestionBoxMobile.Items.Count > 0)
                        suggestionBoxMobile.SelectedIndex = 0;
                    e.Handled = true;
                }
            };

            boxName.TextChanged += BoxName_TextChanged;
            boxMobile.KeyDown += (s, e) =>
            {
                if (suggestionBoxMobile.Visible)
                {
                    if (e.KeyCode == Keys.Down)
                    {
                        suggestionBoxMobile.Focus();
                        suggestionBoxMobile.SelectedIndex = 0;
                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyCode == Keys.Enter)
                    {
                        if (suggestionBoxMobile.SelectedItem != null)
                        {
                            SelectSuggestion(suggestionBoxMobile);
                            e.SuppressKeyPress = true;
                            return;
                        }
                    }
                }

                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    boxName.Focus();   // Move to Name
                }
            };

            suggestionBoxMobile.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Up && suggestionBoxMobile.SelectedIndex == 0)
                {
                    boxMobile.Focus();
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    SelectSuggestion(suggestionBoxMobile);
                    e.Handled = true;
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    suggestionBoxMobile.Visible = false;
                    boxMobile.Focus();
                }
            };

            boxName.KeyDown += (s, e) =>
            {
                if (suggestionBoxName.Visible)
                {
                    if (e.KeyCode == Keys.Down)
                    {
                        suggestionBoxName.Focus();
                        suggestionBoxName.SelectedIndex = 0;
                        e.Handled = true;
                        return;
                    }
                    else if (e.KeyCode == Keys.Enter)
                    {
                        if (suggestionBoxName.SelectedItem != null)
                        {
                            SelectSuggestion(suggestionBoxName);
                            e.SuppressKeyPress = true;
                            return;
                        }
                    }
                }

                //  NEW: UP arrow goes to Mobile
                if (e.KeyCode == Keys.Up)
                {
                    e.SuppressKeyPress = true;
                    boxMobile.Focus(); //  important fix
                    return;
                }


                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    checkBox1.Focus();   // Move to Paper Bill
                }
            };
        }

        private void SuggestionBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();
            var g = e.Graphics;
            var item = ((ListBox)sender).Items[e.Index].ToString();

            var parts = item.Split('|');
            string line1 = parts.Length > 0 ? parts[0].Trim() : "";
            string line2 = parts.Length > 1 ? parts[1].Trim() : ""; 

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                g.FillRectangle(Brushes.LightBlue, e.Bounds);

            g.DrawString(line1, Line1Font, Brushes.Black, e.Bounds.X + 5, e.Bounds.Y + 2);
            g.DrawString(line2, Line2Font, Brushes.Gray, e.Bounds.X + 10, e.Bounds.Y + 22);

            e.DrawFocusRectangle();
        }

        private void BoxMobile_TextChanged(object sender, EventArgs e)
        {
            if (_updatingFields) return;
            string text = boxMobile.Text.Trim();
            if (text.Length < 2)
            {
                suggestionBoxMobile.Visible = false;
                return;
            }

            var matches = contacts
                .Where(c => c.phone.Contains(text))
                .Select(c => $"{c.phone}|{c.name}")
                .ToList();

            ShowSuggestions(suggestionBoxMobile, matches, boxMobile);
        }

        private void BoxName_TextChanged(object sender, EventArgs e)
        {
            if (_updatingFields) return;
            string text = boxName.Text.Trim();
            if (text.Length < 2)
            {
                suggestionBoxName.Visible = false;
                return;
            }

            var matches = contacts
                .Where(c => c.name.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                .Select(c => $"{c.name}|{c.phone}")
                .ToList();

            ShowSuggestions(suggestionBoxName, matches, boxName);
        }

        private void ShowSuggestions(ListBox listBox, List<string> matches, TextBox source)
        {
            if (matches.Count == 0)
            {
                listBox.Visible = false;
                return;
            }

            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (var m in matches)
                listBox.Items.Add(m);
            listBox.EndUpdate();

            var pos = source.PointToScreen(Point.Empty);
            pos = PointToClient(pos);
            listBox.SetBounds(pos.X, pos.Y + source.Height, source.Width, 160);
            listBox.Visible = true;
        }

        private void SelectSuggestion(ListBox listBox)
        {
            if (listBox.SelectedItem == null) return;
            string selected = listBox.SelectedItem.ToString();
            string[] parts = selected.Split('|');
            _updatingFields = true;

            if (listBox == suggestionBoxMobile && parts.Length >= 2)
            {
                boxMobile.Text = parts[0].Trim();
                boxName.Text = parts[1].Trim();
            }
            else if (listBox == suggestionBoxName && parts.Length >= 2)
            {
                boxName.Text = parts[0].Trim();
                boxMobile.Text = parts[1].Trim();
            }

            _updatingFields = false;
            listBox.Visible = false;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_SYSCOMMAND)
            {
                int wParam = m.WParam.ToInt32() & 0xFFF0;
                if (wParam == SC_MINIMIZE)
                    return;
                if (wParam == SC_CLOSE && ModifierKeys == Keys.Alt)
                    return;
            }

            if (m.Msg == WM_KEYDOWN && m.WParam.ToInt32() == VK_ESCAPE)
                return;

            base.WndProc(ref m);
        }

        private void EnterMovesNext(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                this.SelectNextControl(
                    (Control)sender,
                    true,
                    true,
                    true,
                    true
                );
            }
        }

        private void button1_Click(object sender, EventArgs e) { }
        private void boxName_TextChanged_1(object sender, EventArgs e) { }
        private void checkBox1_CheckedChanged(object sender, EventArgs e) { }
        private void checkBox2_CheckedChanged(object sender, EventArgs e) { }

        private void checkbox2_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }
}