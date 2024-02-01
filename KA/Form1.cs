using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KA
{
    public partial class Form1 : Form
    {
        string terminalSTR, neterminalSTR, simvolSTART, startsostNKA, fFORnka, fFORdka;
        int countOfpravila;
        List<string> termLIST, netermLIST, praviloleft, praviloright, konechsostNKA, Peredat, fFORnkaLIST, fFORdkaLIST;
        bool isDFA;
        Dictionary<string, Dictionary<string, List<string>>> transitions;
        List<string> alphabet = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        private void button2_Click(object sender, EventArgs e)
        {
            if (neterminalSTR.IndexOf(textBox2.Text) == -1)
            {
                if (!(neterminalSTR.Length == 2))
                {
                    neterminalSTR = neterminalSTR.Insert(neterminalSTR.Length - 1, "," + textBox2.Text);
                }
                else
                {
                    neterminalSTR = neterminalSTR.Insert(neterminalSTR.Length - 1, textBox2.Text);
                }
            }
            else
                Info(2);
            richTextBox2.Text = neterminalSTR;
            textBox2.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(neterminalSTR.Contains(textBox3.Text))
            {
                simvolSTART = textBox3.Text;
                richTextBox3.Text = simvolSTART;
                textBox3.Clear();
            }
            else
            {
                Info(4);
                textBox3.Clear();
            }
        }
        private void Proverki()
        {
            termLIST = new List<string>();
            netermLIST = new List<string>();
            terminalSTR = terminalSTR.Substring(1, terminalSTR.Length - 2);
            string[] subs = terminalSTR.Split(',');
            for (int i = 0; i < subs.Length; i++)
            {
                termLIST.Add(subs[i]);
            }
            neterminalSTR = neterminalSTR.Substring(1, neterminalSTR.Length - 2);
            string[] subs1 = neterminalSTR.Split(',');
            for (int i = 0; i < subs1.Length; i++)
            {
                netermLIST.Add(subs1[i]);
            }
            praviloleft = new List<string>();
            praviloright = new List<string>();
            for (int i = 0; i < countOfpravila; i++)
            {
                string ab = (panel1.Controls[i.ToString()] as TextBox).Text;
                string ad = (panel1.Controls[(i + 200).ToString()] as TextBox).Text;
                if(ab != "" && ad != "")
                {
                    praviloleft.Add((panel1.Controls[i.ToString()] as TextBox).Text);
                    praviloright.Add((panel1.Controls[(i + 200).ToString()] as TextBox).Text);
                }
                else
                {
                    Info(5);
                    Restart();
                    return;
                }
            }
            int count1 = 0;
            for (int i = 0; i < praviloleft.Count; i++)
            {
                for (int j = 0; j < praviloleft[i].Length; j++)
                {
                    if (netermLIST.Contains(praviloleft[i][j].ToString()))
                    {

                    }
                    else
                        count1++;
                }
                for (int j = 0; j < praviloright[i].Length; j++)
                {
                    if (netermLIST.Contains(praviloright[i][j].ToString()) || termLIST.Contains(praviloright[i][j].ToString()) || praviloright[i][j] == 'e')
                    {

                    }
                    else
                        count1++;
                }
                if (count1 > 0)
                {
                    Info(3);
                    break;
                }
            }
            if (count1 != 0)
                Restart();
        }
        private bool RegularLang()
        {
            for (int i = 0; i < praviloright.Count; i++)
            {
                if (praviloright[i].Length == 1)
                {
                    if (praviloright[i][0] == 'e' || termLIST.Contains(praviloright[i][0].ToString()))
                    { }
                    else
                    {
                        return false;
                    }
                }
                else if (praviloright[i].Length == 2)
                {
                    if (termLIST.Contains(praviloright[i][0].ToString()) && netermLIST.Contains(praviloright[i][1].ToString()))
                    {

                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            return true;
        }
        private void MakeNKA()
        {
            startsostNKA = simvolSTART;
            IEnumerable<string> unusedLetters = alphabet.Except(netermLIST);
            konechsostNKA = new List<string>();
            string sss = unusedLetters.First();
            konechsostNKA.Add(sss);
            for (int i = 0; i < praviloright.Count; i++)
            {
                if (praviloright[i].Length == 1 && praviloright[i][0] != 'e')
                {
                    string d = praviloright[i][0].ToString() + sss;
                    praviloright[i] = d;
                }
            }

            transitions = new Dictionary<string, Dictionary<string, List<string>>>();
            for (int i = 0; i < netermLIST.Count; i++)
            {
                transitions[netermLIST[i]] = new Dictionary<string, List<string>>();
                for (int j = 0; j < termLIST.Count; j++)
                {
                    transitions[netermLIST[i]][termLIST[j]] = new List<string>();
                }
            }
            for (int i = 0; i < praviloright.Count; i++)
            {
                if (praviloright[i][0] == 'e')
                {
                    if (!konechsostNKA.Contains(praviloleft[i]))
                        konechsostNKA.Add(praviloleft[i]);
                }
                else
                {
                    transitions[praviloleft[i]][praviloright[i][0].ToString()].Add(praviloright[i][1].ToString());
                }
            }
            isDFA = true;
            foreach (var state in transitions)
            {
                foreach (var symbol in state.Value)
                {
                    if (symbol.Value.Count != 1)
                    {
                        isDFA = false;
                        break;
                    }
                }
            }
            Dictionary<string, HashSet<string>> graph = new Dictionary<string, HashSet<string>>();
            foreach (var state in transitions)
            {
                graph.Add(state.Key, new HashSet<string>());
            }
            foreach (var state in transitions)
            {
                foreach (var symbol in state.Value)
                {
                    foreach (var nextState in symbol.Value)
                    {
                        if (!graph.ContainsKey(nextState))
                        {
                            graph.Add(nextState, new HashSet<string>());
                        }
                        graph[state.Key].Add(nextState);
                    }
                }
            }
            fFORnka = "K = ({ ";
            foreach (var state in graph)
            {
                fFORnka += state.Key + ",";
            }
            fFORnka = fFORnka.Remove(fFORnka.Length - 1, 1);
            fFORnka += "}, {";
            foreach (var d in termLIST)
            {
                fFORnka += d + ",";
            }
            fFORnka = fFORnka.Remove(fFORnka.Length - 1, 1);
            fFORnka += "}, F, ";
            fFORnka += startsostNKA + ",{";
            foreach(var d in konechsostNKA)
            {
                fFORnka += d + ",";
            }
            fFORnka = fFORnka.Remove(fFORnka.Length - 1, 1);
            fFORnka += "})";
            fFORnkaLIST = new List<string>();
            foreach (var state in transitions)
            {
                fFORnkaLIST.Add("Переход от " + state.Key + " :");
                string vvv = "";
                foreach (var symbol in state.Value)
                {
                    string lll = "";
                    lll = ("По символу " + symbol.Key + " к состоянию: ");
                    string ddd = "";
                    foreach (var nextState in symbol.Value)
                    {
                        lll+=  nextState + " ";
                        ddd += nextState;
                    }
                    
                    if(ddd != "")
                    {
                        vvv += ddd;
                        fFORnkaLIST.Add(lll);
                    }
                    
                }
                if (vvv == "")
                {
                    fFORnkaLIST.RemoveAt(fFORnkaLIST.Count - 1);
                }
            }
            Form form = new Form();
            if(isDFA)
                form.Text = "ДКА";
            else
                form.Text = "НКА";
            form.Width = 800;
            form.Height = 600;
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            form.Controls.Add(pictureBox);

            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
           
            Dictionary<string, PointF> nodePositions = new Dictionary<string, PointF>();
            int nodeRadius = 25;
            float centerX = form.Width / 2;
            float centerY = form.Height / 2;
            int numNodes = graph.Count;
            float angleIncrement = (float)(2 * Math.PI / numNodes);
            float angle1 = 0;
            foreach (var node in graph)
            {
                float x = centerX + (float)(Math.Cos(angle1) * (form.Width / 3));
                float y = centerY + (float)(Math.Sin(angle1) * (form.Height / 3));
                graphics.FillEllipse(Brushes.White, x - nodeRadius, y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                if(konechsostNKA.Contains(node.Key))
                    graphics.DrawEllipse(Pens.Black, x - nodeRadius, y - nodeRadius, nodeRadius * (float)1.8, nodeRadius * (float)1.8);
                if(startsostNKA == node.Key)
                {
                    Pen arrowPen = new Pen(Brushes.Black, 4)
                    {
                        EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor
                    };
                    graphics.DrawLine(arrowPen, x - nodeRadius- 10, y - nodeRadius - 10, x - nodeRadius / 2, y - nodeRadius / 2);
                }
                graphics.DrawEllipse(Pens.Black, x - nodeRadius, y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                graphics.DrawString(node.Key, new Font("Arial", 12), Brushes.Black, x - 10, y - 10);
                nodePositions[node.Key] = new PointF(x, y);
                angle1 += angleIncrement;
            }

            foreach (var node in graph)
            {
                PointF start = nodePositions[node.Key];
                foreach (var neighbor in node.Value)
                {
                    PointF end = nodePositions[neighbor];
                    float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
                    float x1, y1, x2, y2;
                    if (node.Key != neighbor) 
                    {
                        x1 = start.X + nodeRadius * (float)Math.Cos(angle);
                        y1 = start.Y + nodeRadius * (float)Math.Sin(angle);
                        x2 = end.X - nodeRadius * (float)Math.Cos(angle);
                        y2 = end.Y - nodeRadius * (float)Math.Sin(angle);
                    }
                    else
                    {
                        x1 = start.X + nodeRadius * (float)Math.Cos(angle - Math.PI / 4);
                        y1 = start.Y + nodeRadius * (float)Math.Sin(angle - Math.PI / 4);
                        x2 = start.X + nodeRadius * (float)Math.Cos(angle + Math.PI / 4);
                        y2 = start.Y + nodeRadius * (float)Math.Sin(angle + Math.PI / 4);
                    }
                    Pen arrowPen = new Pen(Brushes.Black, 4)
                    {
                        EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor
                    };
                    graphics.DrawLine(arrowPen, x1, y1, x2, y2);
                    string edgeLabel = "";
                    foreach (var symbol in transitions[node.Key])
                    {
                        if (symbol.Value.Contains(neighbor))
                        {
                            edgeLabel += " " + symbol.Key;
                        }
                    }
                    float quarterX = x1 + (x2 - x1) / 4;
                    float quarterY = y1 + (y2 - y1) / 4;
                    PointF labelPoint = new PointF(quarterX, quarterY);
                    graphics.DrawString(edgeLabel, new Font("Arial", 24, FontStyle.Bold), new SolidBrush(Color.Red), labelPoint);
                }
            }
            pictureBox.Image = bitmap;
            form.Show();
        }
        private void MakeDKA()
        {
            var dfa = new Dictionary<string, Dictionary<string, string>>();
            var startStates = new List<string> { startsostNKA};
            var initialState = startStates[0];
            var unprocessedStates = new Queue<string>();
            unprocessedStates.Enqueue(initialState);

            while (unprocessedStates.Count > 0)
            {
                var currentState = unprocessedStates.Dequeue();
                dfa[currentState] = new Dictionary<string, string>();

                var combinedTransitions = new Dictionary<string, List<string>>();
                foreach (var state in currentState.Split(','))
                {
                    if (transitions.ContainsKey(state))
                    {
                        foreach (var symbol in transitions[state])
                        {
                            if (combinedTransitions.ContainsKey(symbol.Key))
                            {
                                combinedTransitions[symbol.Key].AddRange(symbol.Value);
                            }
                            else
                            {
                                combinedTransitions[symbol.Key] = new List<string>(symbol.Value);
                            }
                        }
                    }
                }
                foreach (var symbol in combinedTransitions.Keys)
                {
                    var nextState = string.Join(",", combinedTransitions[symbol].Distinct().OrderBy(s => s));

                    if (!dfa.ContainsKey(nextState))
                    {
                        unprocessedStates.Enqueue(nextState);
                    }
                    dfa[currentState][symbol] = nextState;
                }
            }

            var emptyState = "EMPTY";
            foreach (var state in dfa.Keys)
            {
                foreach (var symbol in transitions.Values.SelectMany(dict => dict.Keys).Distinct())
                {
                    if (!dfa[state].ContainsKey(symbol))
                    {
                        dfa[state][symbol] = emptyState;
                    }
                }
            }
            List<string> states = new List<string>();

            foreach (var state in dfa)
            {
                string ddd = "";
                if (state.Key != "")
                {
                    var keysToRemove = new List<string>();
                    foreach (var symbol in state.Value)
                    {
                        if (symbol.Value == "" || symbol.Value == "EMPTY")
                            keysToRemove.Add(symbol.Key);
                        if(symbol.Value != "" && symbol.Value != "EMPTY")
                            ddd += symbol.Value;
                    }
                    foreach (var key in keysToRemove)
                    {
                        state.Value.Remove(key);
                    }
                }
                if (ddd == "")
                    states.Add(state.Key);
            }
            foreach (var state in dfa)
            {
                if (state.Key == "" || state.Value == null)
                    states.Add(state.Key);
            }

            for (int i = 0; i < states.Count; i++)
                dfa.Remove(states[i]);
            
            Dictionary<string, HashSet<string>> graph = new Dictionary<string, HashSet<string>>();
            string lll = "";
            foreach (var state in dfa)
            {
                if (lll == "")
                    lll = state.Key;
                graph.Add(state.Key, new HashSet<string>());
            }
            foreach (var state in dfa)
            {
                foreach (var symbol in state.Value)
                {
                    if (!graph.ContainsKey(symbol.Value))
                    {
                        graph.Add(symbol.Value, new HashSet<string>());
                    }
                    graph[state.Key].Add(symbol.Value);
                }
            }
            fFORdka = "K` = ({";
            foreach (var state in dfa)
                fFORdka += state.Key + ";";
            fFORdka = fFORdka.Remove(fFORdka.Length - 1, 1);
            fFORdka += "}, {";
            foreach (var d in termLIST)
            {
                fFORdka += d + ";";
            }
            fFORdka = fFORdka.Remove(fFORdka.Length - 1, 1);
            fFORdka += "}, F`, ";
            fFORdka += lll + ",{";
            foreach (var state in graph)
            {
                for(int i = 0; i < konechsostNKA.Count; i++)
                {
                    if (state.Key.Contains(konechsostNKA[i]))
                        fFORdka += state.Key + ";";
                }
            }
            fFORdka = fFORdka.Remove(fFORdka.Length - 1, 1);
            fFORdka += "})";
            fFORdkaLIST = new List<string>();
            foreach (var state in dfa)
            {
                fFORdkaLIST.Add($"Переход от  {state.Key}: ");
                foreach (var symbol in state.Value)
                {
                    fFORdkaLIST.Add($"По символу {symbol.Key} к состоянию {symbol.Value} ");
                }
            }
            Form form = new Form();
            form.Text = "ДКА";
            form.Width = 800;
            form.Height = 600;
            PictureBox pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Fill;
            form.Controls.Add(pictureBox);

            Bitmap bitmap = new Bitmap(pictureBox.Width, pictureBox.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Dictionary<string, PointF> nodePositions = new Dictionary<string, PointF>();
            int nodeRadius = 25;
            float centerX = form.Width / 2;
            float centerY = form.Height / 2;
            int numNodes = graph.Count;
            float angleIncrement = (float)(2 * Math.PI / numNodes);
            float angle1 = 0;
            foreach (var node in graph)
            {
                float x = centerX + (float)(Math.Cos(angle1) * (form.Width / 3));
                float y = centerY + (float)(Math.Sin(angle1) * (form.Height / 3));
                graphics.FillEllipse(Brushes.White, x - nodeRadius, y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                graphics.DrawEllipse(Pens.Black, x - nodeRadius, y - nodeRadius, nodeRadius * 2, nodeRadius * 2);
                string name = node.Key;
                for(int v = 0; v < name.Length;v++)
                {
                    if (konechsostNKA.Contains(name[v].ToString()))
                    {
                        graphics.DrawEllipse(Pens.Black, x - nodeRadius, y - nodeRadius, nodeRadius * (float)1.8, nodeRadius * (float)1.8);
                        break;
                    }
                }
                if (startsostNKA == node.Key)
                {
                    Pen arrowPen = new Pen(Brushes.Black, 4)
                    {
                        EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor
                    };
                    graphics.DrawLine(arrowPen, x - nodeRadius - 10, y - nodeRadius - 10, x - nodeRadius / 2, y - nodeRadius / 2);
                }
                graphics.DrawString(node.Key, new Font("Arial", 12), Brushes.Black, x - 10, y - 10);
                nodePositions[node.Key] = new PointF(x, y);
                angle1 += angleIncrement;
            }

            Random rnd = new Random();
            foreach (var node in graph)
            {
                PointF start = nodePositions[node.Key];
                foreach (var neighbor in node.Value)
                {
                    PointF end = nodePositions[neighbor];
                    float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
                    float x1, y1, x2, y2;
                    if (node.Key != neighbor)
                    {
                        x1 = start.X + nodeRadius * (float)Math.Cos(angle);
                        y1 = start.Y + nodeRadius * (float)Math.Sin(angle);
                        x2 = end.X - nodeRadius * (float)Math.Cos(angle);
                        y2 = end.Y - nodeRadius * (float)Math.Sin(angle);
                    }
                    else
                    {
                        x1 = start.X + nodeRadius * (float)Math.Cos(angle - Math.PI / 4);
                        y1 = start.Y + nodeRadius * (float)Math.Sin(angle - Math.PI / 4);
                        x2 = start.X + nodeRadius * (float)Math.Cos(angle + Math.PI / 4);
                        y2 = start.Y + nodeRadius * (float)Math.Sin(angle + Math.PI / 4);
                    }
                    Pen arrowPen = new Pen(Brushes.Black, 4);
                    arrowPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                    graphics.DrawLine(arrowPen, x1, y1, x2, y2);
                    string edgeLabel = "";
                    foreach (var symbol in dfa[node.Key])
                    {
                        if (symbol.Value.Contains(neighbor))
                        {
                            edgeLabel += " " + symbol.Key;
                        }
                    }
                    float quarterX = x1 + (x2 - x1) / 4;
                    float quarterY = y1 + (y2 - y1) / 4;
                    PointF labelPoint = new PointF(quarterX, quarterY);
                    graphics.DrawString(edgeLabel, new Font("Arial", 24, FontStyle.Bold), new SolidBrush(Color.Red), labelPoint);
                }
            }
            pictureBox.Image = bitmap;
            form.Show();
        }

        private void WriteAll(int a)
        {
            switch (a)
            {
                case 0:
                    Peredat.Add("1.Проверка грамматики на принадлежность к классу регулярных грамматик.");
                    Peredat.Add("Результат:");
                    Peredat.Add("Грамматика принадлежит к классу регулярных грамматик.");
                    break;
                case 1:
                    Peredat.Add("2.Был построен НКА:");
                    Peredat.Add(fFORnka);
                    Peredat.Add("F:");
                    foreach (var d in fFORnkaLIST)
                        Peredat.Add(d);
                        break;
                case 2:
                    Peredat.Add("2.Был построен ДКА:");
                    Peredat.Add(fFORnka);
                    Peredat.Add("F:");
                    foreach (var d in fFORnkaLIST)
                        Peredat.Add(d);
                    break;
                case 3:
                    Peredat.Add("3.Был построен ДКА:");
                    Peredat.Add(fFORdka);
                    Peredat.Add("F`:");
                    foreach (var d in fFORdkaLIST)
                        Peredat.Add(d);
                    break;
                default:
                    break;
            }
            if(a == 0)
            {
                string net = "Список нетерминалов: ";
                string t = "Список терминалов: ";
                string pravila;
                for (int i = 0; i < netermLIST.Count; i++)
                {
                    net += netermLIST[i].ToString() + " ";
                }
                for (int i = 0; i < termLIST.Count; i++)
                {
                    t += termLIST[i].ToString() + " ";
                }
                Peredat.Add(net);
                Peredat.Add(t);
                Peredat.Add("Правила: ");
                for (int i = 0; i < praviloleft.Count; i++)
                {
                    pravila = praviloleft[i] + " -> " + praviloright[i];
                    Peredat.Add(pravila);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            terminalSTR = "{}";
            richTextBox1.Text = terminalSTR;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            neterminalSTR = "{}";
            richTextBox2.Text = neterminalSTR;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Info(7);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Info(6);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            countOfpravila = Convert.ToInt32(numericUpDown1.Value);
            for (int i = 0; i < countOfpravila; i++)
            {
                panel1.Controls.Add(new TextBox() { Name = i.ToString(), Location = new Point(10, i * 30) });
                panel1.Controls.Add(new Label() { Name = (i + 100).ToString(), Location = new Point(120, i * 30), Text = "⟶", Width = 20 });
                panel1.Controls.Add(new TextBox() { Name = (i + 200).ToString(), Location = new Point(150, i * 30) });
            }
            button10.Show();
            button11.Show();
            button6.Hide();
        }
        private void Restart()
        {
            button10.Hide();
            button11.Hide();
            terminalSTR = "{}";
            neterminalSTR = "{}";
            simvolSTART = "S";
            richTextBox3.Text = simvolSTART;
            richTextBox1.Text = terminalSTR;
            richTextBox2.Text = neterminalSTR;
            panel1.Controls.Clear();
            numericUpDown1.Value = 1;
            button6.Show();
            Peredat.Clear();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Restart();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button11.Show();
            termLIST = new List<string>() { "0", "1", "a", "b", "c" };
            netermLIST = new List<string>() { "E", "A", "B", "C", "D" };
            praviloleft = new List<string>() {"E", "E", "A", "A", "B", "B", "B",
            "D","D","D"};
            praviloright = new List<string>() { "0A", "e", "aB", "aD", "bB",
                "1C", "c","aD", "0C", "c"};
            simvolSTART = "E";
            if (RegularLang())
            {
                WriteAll(0);
            }
            else
            {
                Info(8);
                Restart();
                return;
            }
            MakeNKA();
            if (!isDFA)
            {
                MakeDKA();
                WriteAll(1);
                WriteAll(3);
            }
            else
            {
                WriteAll(2);
            }
            Form2 frm = new Form2(Peredat);
            frm.Show();
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Proverki();
            if(RegularLang())
            {
                WriteAll(0);
            }
            else
            {
                Info(8);
                Restart();
                return;
            }
            MakeNKA();
            if (!isDFA)
            {
                MakeDKA();
                WriteAll(1);
                WriteAll(3);
            }
            else
            {
                WriteAll(2);
            }
            Form2 frm = new Form2(Peredat);
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (terminalSTR.IndexOf(textBox1.Text) == -1)
            {
                if (!(terminalSTR.Length == 2))
                {
                    terminalSTR = terminalSTR.Insert(terminalSTR.Length - 1, "," + textBox1.Text);
                }
                else
                {
                    terminalSTR = terminalSTR.Insert(terminalSTR.Length - 1, textBox1.Text);
                }
            }
            else
                Info(1);
            richTextBox1.Text = terminalSTR;
            textBox1.Clear();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button10.Hide();
            button11.Hide();
            terminalSTR = "{}";
            neterminalSTR = "{}";
            simvolSTART = "S";
            richTextBox3.Text = simvolSTART;
            richTextBox1.Text = terminalSTR;
            richTextBox2.Text = neterminalSTR;
            panel1.AutoScroll = true;
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            richTextBox3.ReadOnly = true;
            button6.Show();
            Peredat = new List<string>();
        }
        private void Info(int choose)
        {
            switch (choose)
            {
                case 1:
                    {
                        MessageBox.Show(
                            "В списке терминалов уже находится вводимый символ.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                case 2:
                    {
                        MessageBox.Show(
                            "В списке нетерминалов уже находится вводимый символ.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                case 3:
                    {
                        MessageBox.Show(
                            "Правила содержат символы, несодержащиеся ни в терминалах, ни в нетерминалах. И не равные e.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                case 4:
                    {
                        MessageBox.Show(
                            "Символа нет в списке нетерминалов.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                case 5:
                    {
                        MessageBox.Show(
                            "Присутствуют пустые правила.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                case 6:
                    {
                        MessageBox.Show(
                            "Колач Андрей 3221",
                            "Автор",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                case 7:
                    {
                        MessageBox.Show(
                            "1.Задаёте набор терминальных символов.\r\n" +
                            "2.Задаёте набор нетерминальных символов.\r\n" +
                            "3.Задаёте стартовый символ грамматики.\r\n" +
                            "4.Задаёте количество правил вывода. Нажимаете кнопку 'Добавить'.\r\n" +
                            "5.Вводите правила вывода.\r\n" +
                            "6.Нажимаете кнопку 'Старт'.\r\n" +
                            "7.Кнопка '6 вариант' записывает значения 6 варианта.\r\n" +
                            "8.Для возвращения в исходное состояние нажимаете кнопку 'Сброс'.\r\n" +
                            "9.Пустой символ добавляется через e. Если в языке используется этот символ, его необходимо заменить.",
                            "Помощь.",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                case 8:
                    {
                        MessageBox.Show(
                            "Грамматика не относится к классу регулярных грамматик.",
                            "Ошибка",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information,
                            MessageBoxDefaultButton.Button1);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
