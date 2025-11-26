using System;
using System.Collections;
using System.Configuration;
using System.Text;
using System.Web;
using System.Xml;
using Tacton.Configurator.ObjectModel;

namespace TactonUtils
{
    [Serializable]
    public class SafeState
    {
        private string model;
        private string[] dataIDs;
        private string application;
        private SequencedHashtable steps = new SequencedHashtable();

        public string Model { get { return this.model; } set { this.model = value; } }
        public string[] DataIDs { get { return this.dataIDs; } set { this.dataIDs = value; } }
        public string Application { get { return this.application; } set { this.application = value; } }
        public SequencedHashtable Steps { get { return this.steps; } set { this.steps = value; } }

        public SafeState(string model, string[] dataIDs, string application)
        {
            this.model = model;
            this.dataIDs = dataIDs;
            this.application = application;
            this.steps = new SequencedHashtable();
        }

        public SafeState(string safestate)
        {
            if (safestate != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(safestate);
                this.Init(doc);
            }
        }

        public SafeState(XmlDocument doc)
        {
            this.Init(doc);
        }

        private static string Trunc(string p)
        {
            int idx = p.IndexOf(".");
            if (idx >= 0) p = p.Substring(0, idx);
            return p;
        }

        public static SafeState GetSafeState(string oldstate, string shapeGroup, Hashtable productInputTable, Hashtable userInputTable, Hashtable shapeInputTable)
        {
            SafeState result = new SafeState(oldstate);
            try
            {
                if (productInputTable != null)
                {
                    foreach (DictionaryEntry item in productInputTable)
                    {
                        result.SetCommit(shapeGroup + "_step", shapeGroup + item.Key.ToString().Trim(), item.Value.ToString().Trim());
                    }
                }
                if (userInputTable != null)
                {
                    foreach (DictionaryEntry item in userInputTable)
                    {
                        result.SetCommit(shapeGroup + "_step", shapeGroup + item.Key.ToString().Trim(), item.Value.ToString().Trim());
                    }
                }
                if (shapeInputTable != null)
                {
                    foreach (DictionaryEntry item in shapeInputTable)
                    {
                        result.SetCommit(shapeGroup + "_step", shapeGroup + item.Key.ToString().Trim(), item.Value.ToString().Trim());
                    }
                }
            }
            catch 
            {
                throw;
            }
            return result;
        }


        public void Init(XmlDocument doc)
        {
            this.model = GetStringValue("/state/model", doc);
            ArrayList alDataIDs = new ArrayList();
            foreach (XmlNode nDataID in doc.SelectNodes("/state/dataids/dataid"))
            {
                alDataIDs.Add(nDataID.InnerText);
            }
            this.dataIDs = (string[])alDataIDs.ToArray(typeof(string));
            this.application = GetStringValue("/state/application", doc);
            this.steps = new SequencedHashtable();
            foreach (XmlNode nStep in doc.SelectNodes("/state/safecookie/safe-step"))
            {
                string sStepName = nStep.Attributes["name"].InnerText;
                foreach (XmlNode nPar in nStep.SelectNodes("./commits/committed"))
                {
                    this.SetCommit(sStepName, nPar.Attributes["name"].InnerText, nPar.InnerText);
                }
            }
        }

        public static string GetStringValue(string xpath, XmlDocument doc)
        {
            XmlNode n = doc.SelectSingleNode(xpath);
            if (n != null)
            {
                return n.InnerText;
            }
            return "";
        }

        public ArrayList GetCommits(string stepName)
        {
            ArrayList commits = (ArrayList)this.steps[stepName];
            if (commits == null)
            {
                commits = new ArrayList();
                this.steps.Add(stepName, commits);
            }
            return commits;
        }

        public bool SetCommit(string fieldName, string val)
        {
            foreach (ArrayList commits in this.steps)
            {
                foreach (string[] arr in commits)
                {
                    if (arr[0] == fieldName)
                    {
                        arr[1] = val;
                        return true;
                    }
                }
            }
            return false;
        }

        public void SetCommit(string stepName, string fieldName, string val)
        {
            ArrayList commits = this.GetCommits(stepName);
            SetCommit(fieldName, val, commits);
        }

        private static void SetCommit(string fieldName, string val, ArrayList commits)
        {
            foreach (string[] arr in commits)
            {
                if (arr[0] == fieldName)
                {
                    arr[1] = val;
                    return;
                }
            }
            commits.Add(new string[] { fieldName, val });
        }

        public override string ToString()
        {
            string firstStep = null;
            // string firstStepName = null;
            StringBuilder sb = new StringBuilder();
            StringBuilder sbStep = new StringBuilder();
            sb.Append("<state>").Append("<model>").Append(this.model).Append("</model>").
                Append("<dataids>");
            foreach (string dataID in this.dataIDs)
            {
                sb.Append("<dataid>").Append(dataID).Append("</dataid>");
            }
            sb.Append("</dataids>").
                Append("<application>").Append(this.application).Append("</application>").
                Append("<safecookie>");
            if (steps.Count > 0)
            {
                foreach (string key in this.steps.Keys)
                {
                    sbStep.Length = 0;
                    ArrayList commits = (ArrayList)this.steps[key];
                    sbStep.Append("<safe-step name=\"").Append(key).Append("\">").Append("<commits>");
                    foreach (string[] committed in commits)
                    {
                        if ((committed[0] != null) && (committed[1] != null))
                        {
                            firstStep = "";
                            sbStep.Append("<committed name=\"").Append(committed[0]).Append("\">");
                            sbStep.Append(committed[1]);
                            sbStep.Append("</committed>");
                        }
                    }
                    sbStep.Append("</commits></safe-step>");
                    if (firstStep != null)
                    {
                        // Add newly created step to the stringbuilder.
                        sb.Append(sbStep.ToString());
                    }
                }
            }
            sb.Append("</safecookie>");
            sb.Append("<steps />");
            sb.Append("</state>");
            return sb.ToString();
        }
    }

}
