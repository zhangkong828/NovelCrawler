using NovelCrawler.Common;
using NovelCrawler.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelCrawler.Rule
{
    public partial class RuleForm : Form
    {
        static readonly string _directoryPath = AppDomain.CurrentDomain.BaseDirectory + "Rules";

        static Dictionary<string, RuleModel> _rules;
        static string _currentSelectRuleKey;//当前选择的规则key
        static string _currentSelectProperty;//当前选择的规则属性名称

        public RuleForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;

            _rules = new Dictionary<string, RuleModel>();

        }


        private void RuleForm_Load(object sender, EventArgs e)
        {

            //加载Rules目录下的所有规则
            if (!Directory.Exists(_directoryPath))
            {
                MessageBox.Show("Rules目录不存在");
                return;
            }

            var files = Directory.GetFiles(_directoryPath, "*.xml");
            if (files.Length == 0)
            {
                MessageBox.Show("Rules目录下未找到相关xml规则文件");
                return;
            }

            foreach (var file in files)
            {
                try
                {
                    var filename = Path.GetFileName(file);
                    if (_rules.ContainsKey(filename))
                    {
                        MessageBox.Show("不能出现名称相同的xml规则文件");
                        return;
                    }
                    var rule = XmlHelper.XmlDeserializeFromFile<RuleModel>(file, Encoding.UTF8);
                    comboxRules.Items.Add(filename);
                    _rules.Add(filename, rule);
                }
                catch { }
            }

            if (_rules.Count == 0 || comboxRules.Items.Count == 0)
            {
                MessageBox.Show("xml规则文件加载失败");
                return;
            }
            else
            {
                comboxRules.SelectedIndex = 0;
                _currentSelectRuleKey = comboxRules.SelectedItem.ToString();
            }

            //初始化 属性列表
            Type rt = typeof(RuleModel);
            foreach (var p in rt.GetProperties())
            {
                listBoxRule.Items.Add(p.Name);
            }

            listBoxRule.SelectedIndex = 0;
        }

        #region UI 操作

        /// <summary>
        /// 测试按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestRule_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_currentSelectRuleKey))
                return;

            var fm = new TestForm(_rules[_currentSelectRuleKey]);
            fm.ShowDialog();
        }

        /// <summary>
        /// 保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveRule_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in _rules)
                {
                    var path = _directoryPath + "\\" + item.Key;
                    var rule = item.Value;
                    XmlHelper.XmlSerializeToFile(rule, path, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 规则列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxRules_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(_currentSelectProperty))//第一次是没有的
                {
                    //切换之前，保存 当前的属性
                    SaveCurrentRuleProperty(_currentSelectProperty);
                }

                _currentSelectRuleKey = comboxRules.SelectedItem.ToString();
                listBoxRule.SetSelected(0, true);
            }
            catch { }
        }

        /// <summary>
        /// 规则属性列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_currentSelectRuleKey))
                    return;

                if (!string.IsNullOrEmpty(_currentSelectProperty))//第一次是没有的
                {
                    //保存 改变之前的属性
                    SaveCurrentRuleProperty(_currentSelectProperty);
                }

                var rule = _rules[_currentSelectRuleKey];
                var type = rule.GetType();
                _currentSelectProperty = listBoxRule.SelectedItem.ToString();//当前选中后的属性名
                var p = type.GetProperty(_currentSelectProperty);
                var value = p.GetValue(rule);
                //规则名称
                var attributes = p.GetCustomAttributes(typeof(RuleDescriptionAttribute), true);
                foreach (var attribute in attributes)
                {
                    var attr = attribute as RuleDescriptionAttribute;
                    if (attr != null)
                    {
                        txtRuleName.Text = attr.Name;
                        rtxtRuleDescription.Text = attr.Description;
                        break;
                    }
                }
                //采集规则，替换规则
                if (p.PropertyType.Name == "String")
                {
                    txtRuleFilter.Enabled = false;
                    txtRulePattern.Text = value?.ToString();
                }
                else if (p.PropertyType.Name == "PatternItem")
                {
                    txtRuleFilter.Enabled = true;
                    var pattern = value as PatternItem;
                    txtRulePattern.Text = pattern.Pattern;
                    txtRuleFilter.Text = pattern.Filter;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion


        private void SaveCurrentRuleProperty(string property)
        {
            var rule = _rules[_currentSelectRuleKey];
            var type = rule.GetType();

            var p = type.GetProperty(property);
            var obj = new object();
            if (p.PropertyType.Name == "String")
            {
                obj = txtRulePattern.Text;
            }
            else if (p.PropertyType.Name == "PatternItem")
            {
                var val = new PatternItem();
                val.Pattern = txtRulePattern.Text;
                val.Filter = txtRuleFilter.Text;
                obj = val;
            }

            p.SetValue(rule, obj);
            _rules[_currentSelectRuleKey] = rule;
        }


    }
}
