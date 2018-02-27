using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using GroupBuilder;

namespace GrouperApp
{
    public partial class _Default : Page
    {
        private static bool TEST_FLAG = false;

        private string _GUID;
        protected string GUID
        {
            get
            {
                _GUID = "";
                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    _GUID = Request.QueryString["id"].Trim().ToLower();
                }
                return _GUID;
            }
            set
            {
                _GUID = value;
            }
        }

        private Student _Student;
        protected Student ThisStudent
        {
            get
            {
                if (_Student == null)
                {
                    // Try to get the student by the GUID
                    if (!String.IsNullOrEmpty(GUID))
                    {
                        _Student = GrouperMethods.GetStudentByGUID(GUID);
                    } else // Bad GUID - so, give them back an empty student
                    {
                        _Student = new Student();
                    }

                }
                return _Student;
            }
            set
            {
                _Student = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ThisStudent == null)
            {
                ThisStudent = new Student();
            }

            if (!TEST_FLAG)
            {
                if (ThisStudent.SurveySubmittedDate != null)
                {
                    Response.Redirect("Oops.aspx");
                }
            }

            if (!IsPostBack)
            {
                FirstNameLabel.Text = ThisStudent.FirstName;
                LastNameLabel.Text = ThisStudent.LastName;
                PreferedNameTextBox.Text = ThisStudent.FirstName;

                ClassesRepeater_BindRepeater();
                RolesRepeater_BindRepeater();
                LanguagesRepeater_BindRepeater();
                SkillsRepeater_BindRepeater();

                CurrentCoursesDropDownList.DataSource = GrouperMethods.GetCourses();
                CurrentCoursesDropDownList.DataBind();
            }

        }

        protected void ClassesRepeater_BindRepeater()
        {
            List<Course> courses = GrouperMethods.GetCourses().Where(x => x.CoreCourseFlag == true).ToList();

            ClassesRepeater.DataSource = courses;
            ClassesRepeater.DataBind();
        }

        protected void RolesRepeater_BindRepeater()
        {
            List<Role> roles = GrouperMethods.GetRoles();

            RolesRepeater.DataSource = roles;
            RolesRepeater.DataBind();

        }

        protected void LanguagesRepeater_BindRepeater()
        {
            List<ProgrammingLanguage> languages = GrouperMethods.GetLanguages();

            LanguagesRepeater.DataSource = languages;
            LanguagesRepeater.DataBind();
        }

        protected void SkillsRepeater_BindRepeater()
        {
            List<Skill> skills = GrouperMethods.GetSkills();

            SkillsRepeater.DataSource = skills;
            SkillsRepeater.DataBind();
        }

        protected void SubmitLinkButton_Click(object sender, EventArgs e)
        {
            Student student = ThisStudent;

            //Set submit time
            student.SurveySubmittedDate = DateTime.Now;

            //Set prefered name
            student.PreferredName = PreferedNameTextBox.Text;

            //Set UOID
            student.UOID = int.Parse(UOIDTextBox.Text);

            //Set Primary/Secondary language info
            if (SecondLanguageDropDownList.SelectedIndex > 0)
            {
                student.EnglishSecondLanguageFlag = true;
                student.NativeLanguage = SecondLangTextBox.Text;
            }

            //Set current courses
            if (ViewState["CurrentCourses"] != null)
            {
                foreach (Course curCourse in ((List<Course>)ViewState["CurrentCourses"]))
                {
                    student.CurrentCourses.Add(curCourse);
                }
            }

            //Set prior courses
            foreach (RepeaterItem courseItem in ClassesRepeater.Items)
            {
                HiddenField courseIdHiddenField = (HiddenField)courseItem.FindControl("CourseIdHiddenField");
                int courseID = int.Parse(courseIdHiddenField.Value);
                Course course = GrouperMethods.GetCourse(courseID);

                DropDownList courseGradeDropDownList = (DropDownList)courseItem.FindControl("GradeDropDownList");
                int grade = int.Parse(courseGradeDropDownList.SelectedValue);

                if (grade > 0)
                {
                    course.Grade = grade;
                    student.PriorCourses.Add(course);
                }
            }

            //Set Prefered Roles
            foreach (RepeaterItem roleItem in RolesRepeater.Items)
            {
                HiddenField roleIDHiddenField = (HiddenField)roleItem.FindControl("RoleIDHiddenField");
                int roleID = int.Parse(roleIDHiddenField.Value);
                Role role = GrouperMethods.GetRole(roleID);

                DropDownList roleInterestDropDownList = (DropDownList)roleItem.FindControl("InterestDropDownList");
                int interestLevel = int.Parse(roleInterestDropDownList.SelectedValue);

                if (interestLevel > 0)
                {
                    role.InterestLevel = interestLevel;
                    student.InterestedRoles.Add(role);
                }
            }

            //Set languages
            foreach (RepeaterItem languageItem in LanguagesRepeater.Items)
            {
                HiddenField languageHiddenField = (HiddenField)languageItem.FindControl("LanguageIDHiddenField");
                int languageID = int.Parse(languageHiddenField.Value);
                ProgrammingLanguage language = GrouperMethods.GetLanguage(languageID);

                DropDownList languageDropDownList = (DropDownList)languageItem.FindControl("LanguageDropDownList");
                int languageProficiency = int.Parse(languageDropDownList.SelectedValue);

                if (languageProficiency > 0)
                {
                    language.ProficiencyLevel = languageProficiency;
                    student.Languages.Add(language);
                }
            }

            //Set skills
            foreach (RepeaterItem skillItem in SkillsRepeater.Items)
            {
                HiddenField skillHiddenField = (HiddenField)skillItem.FindControl("SkillIDHiddenField");
                int skillID = int.Parse(skillHiddenField.Value);
                Skill skill = GrouperMethods.GetSkill(skillID);

                DropDownList skillDropDownList = (DropDownList)skillItem.FindControl("SkillDropDownList");
                int skillProficiency = int.Parse(skillDropDownList.SelectedValue);

                //Check for Outgoing Level
                if (skill.Name == "OutgoingLevel")
                {
                    student.OutgoingLevel = skill.ProficiencyLevel;
                }
                else
                {
                    skill.ProficiencyLevel = skillProficiency;
                    student.Skills.Add(skill);
                }
            }

            //Get dev. experience
            student.DevelopmentExperience = DevelopmentExperienceTextBox.Text;

            //Get learning expectations
            student.LearningExpectations = LearningExpectationsTextBox.Text;

            //Update Database
            GrouperMethods.UpdateStudent(student);

            //Redirect to Thanks page
            string idString = "?id=" + ThisStudent.StudentID.ToString();
            Response.Redirect("ThankYou.aspx" + idString);
        }

        protected void CurrentCoursesDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentCoursesDropDownList.SelectedIndex > 0)
            {
                int courseID = int.Parse(CurrentCoursesDropDownList.SelectedValue);

                Course course = GrouperMethods.GetCourse(courseID);
                
                if (ViewState["CurrentCourses"] == null)
                {
                    ViewState["CurrentCourses"] = new List<Course>();
                }
                else //Check if the use is trying to select a course they've already selected
                {
                    foreach (Course curCourse in ((List<Course>)ViewState["CurrentCourses"]))
                    {
                        if (curCourse.CourseID == courseID) //They've already selected this course, so don't re-add it
                        {
                            return;
                        }
                    }
                }

                ((List<Course>)ViewState["CurrentCourses"]).Add(course);

                CurrentCoursesGridView.DataSource = (List<Course>)ViewState["CurrentCourses"];
                CurrentCoursesGridView.DataBind();
            }
        }

        protected void SecondLangDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SecondLanguageDropDownList.SelectedIndex > 0)
            {
                SecondLangTextBoxLabel.Text = "Please enter your primary language:";
                SecondLangTextBoxLabel.Visible = true;
                SecondLangTextBox.Visible = true;

            }
            else
            {
                SecondLangTextBoxLabel.Visible = false;
                SecondLangTextBox.Visible = false;
            }
        }

        protected void CurrentCoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_course")
            {
                int courseID = int.Parse(e.CommandArgument.ToString());

                if (ViewState["CurrentCourses"] != null)
                {
                    ((List<Course>)ViewState["CurrentCourses"]).RemoveAll(x => x.CourseID == courseID);

                    CurrentCoursesGridView.DataSource = (List<Course>)ViewState["CurrentCourses"];
                    CurrentCoursesGridView.DataBind();
                }
            }
        }
    }
}