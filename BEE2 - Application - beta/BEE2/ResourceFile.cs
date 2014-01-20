using System;
using System.Collections.Generic;

namespace BEE2
{
    public class ResourceFile
    {
        /// <summary> Removes inline comments and returns the result </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public static string[] RemoveComments(string[] body)
        {
            for (int i = 0; i < body.Length; i++)
            {
                body[i] = body[i].Trim();
                if (body[i].Contains("//"))
                    body[i] = body[i].Remove(body[i].IndexOf("//"));
            }
            return body;
        }

        /// <summary> Locates the value of the property given in the given body </summary>
        /// <param name="body">Body of text to search through</param>
        /// <param name="property">Property to search for in the body</param>
        /// <returns>Returns the value of the property given</returns>
        public static string FindValue(string[] body, string property)
        {
            string line = "";
            try
            {
                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found! What is its value?
                            //make sure that there is even room for there to be a value
                            if (line.Length >= property.Length + 5)
                            {
                                string value = line.Substring(property.Length + 2).Trim();
                                return value.Substring(1, value.Length - 2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.LogProblem("Exception while trying to find \"" + property + "\"" + Environment.NewLine + ex.ToString());
            }
            return null;
        }

        /// <summary> Locates and alters the value of the property given in the given body </summary>
        /// <param name="body">Body of text to change</param>
        /// <param name="property">Property to change</param>
        /// <param name="value">New value</param>
        /// <returns>Returns the altered body</returns>
        public static string[] ChangeValue(string[] body, string property, string value)
        {
            string line = "";
            try
            {
                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found! What is its value?
                            body[i] = "\t\"" + property + "\"\t\"" + value + "\"";
                            return body;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.LogProblem("Exception while trying to change \"" + property + "\" to \"" + value + "\"" +
                    Environment.NewLine + ex.ToString());
            }
            return null;
        }

        /// <summary> Changes the (numInstance)th instance of the value to property, starting with 0th </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="numInstance"></param>
        /// <returns></returns>
        public static string[] ChangeValue(string[] body, string property, string value, int numInstance)
        {
            string line = "";
            int X = 0;
            try
            {

                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            if (X++ < numInstance)
                                continue;
                            //property found! What is its value?

                            body[i] = "\t\"" + property + "\"\t\"" + value + "\"";
                            return body;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.LogProblem("Exception while trying to change \"" + property + "\" to \"" + value + "\"" +
                    Environment.NewLine + ex.ToString());
            }
            return null;
        }

        /// <summary> Finds the bracketed values enclosed within the target value </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string[] FindSubValue(string[] body, string property)
        {
            string line = "";
            try
            {

                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found!
                            List<string> contents = new List<string>();
                            int braceDepth = 0;
                            int j = i + 1;
                            do
                            {
                                line = body[j].Trim();

                                if (line == "{")
                                    braceDepth++;
                                else if (line == "}")
                                    braceDepth--;
                                //else
                                contents.Add(line);
                                j++;
                            } while (braceDepth > 0);

                            return contents.ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.LogProblem("Exception while trying to find \"" + property + "\"" + Environment.NewLine + ex.ToString());
            }
            return null;
        }

        /// <summary> Finds all instances of the bracketed values enclosed within the target values </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string[][] FindSubValues(string[] body, string property)
        {
            string line = "";
            List<string[]> returnValue = new List<string[]>();
            try
            {
                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found!
                            List<string> contents = new List<string>();
                            int braceDepth = 0;
                            int j = i + 1;
                            do
                            {
                                line = body[j].Trim();

                                if (line == "{")
                                    braceDepth++;
                                else if (line == "}")
                                    braceDepth--;
                                //else
                                contents.Add(line);
                                j++;
                            } while (braceDepth > 0);

                            returnValue.Add(contents.ToArray());
                        }
                    }
                }
                if (returnValue.Count > 0)
                    return returnValue.ToArray();
            }
            catch (Exception ex)
            {
                Global.LogProblem("Exception while trying to find \"" + property + "\"" + Environment.NewLine + ex.ToString());
            }
            return null;
        }

        /// <summary> Returns the passed body with each property changed to the string[] values  </summary>
        /// <param name="body"></param>
        /// <param name="property"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[] ChangeSubValues(string[] body, string property, string[] values)
        {
            string line = "";
            int valuesindex = 0;
            try
            {
                for (int i = 0; i < body.Length; i++)
                {
                    line = body[i].Trim();
                    //make sure this whole line is long enough to even have a property this long
                    if (line.Length > property.Length + 1)
                    {
                        if (line.Substring(1, property.Length + 1).Equals(property + "\""))
                        {
                            //property found!

                            //change the property
                            ////dont output this line if this item is not on the palette at all
                            //if (values[valuesindex] != Global.NotOnPaletteEntry)
                            //{
                            body[i] = body[i].Substring(0, body[i].IndexOf("\""));
                            body[i] += "\"" + property + "\" \"" + values[valuesindex] + "\"";
                            //}
                            //else
                            //{
                            //    body[i] = "//removed position definition";// "//\"Position\"\t\"-1 -1 -1\"";
                            //}
                            valuesindex++;
                            //valuesindex = valuesindex >= values.Length ? 0 : valuesindex;//start the valuesindex over if it runs out of values
                        }
                    }
                }
                return body;
            }
            catch (Exception ex)
            {
                Global.LogProblem("Exception while trying to find \"" + property + "\"" + Environment.NewLine + ex.ToString());
            }
            return null;
        }
    }
}
