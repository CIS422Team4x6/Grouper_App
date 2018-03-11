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
        //Used for testing page w/o an actual student
        private static bool TEST_FLAG = true;

        //GUID is a unique identifier for each student.
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

        //Object representing student's information
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

        //This executes whenever the page is loaded (or re-loaded)
        protected void Page_Load(object sender, EventArgs e)
        {
            //instantiate the student object
            if (ThisStudent == null)
            {
                ThisStudent = new Student();
            }

            if (!TEST_FLAG)
            {
                //If the GUID does not idicate a valid student, redirect
                if ((ThisStudent.SurveySubmittedDate != null) || (GUID == "") || (GUID == null))
                {
                    Response.Redirect("Oops.aspx");
                }
            }

            //Only do these things the firsrt time the page loads (not on post-backs)
            if (!IsPostBack)
            {
                //Fill out name information
                FirstNameLabel.Text = ThisStudent.FirstName;
                LastNameLabel.Text = ThisStudent.LastName;
                PreferedNameTextBox.Text = ThisStudent.FirstName;

                //Bind data for drop-down repeaters
                ClassesRepeater_BindRepeater();
                RolesRepeater_BindRepeater();
                LanguagesRepeater_BindRepeater();
                SkillsRepeater_BindRepeater();
                
                //Bind data for current courses section
                CurrentCoursesDropDownList.DataSource = GrouperMethods.GetCourses();
                CurrentCoursesDropDownList.DataBind();
                CurrentCoursesGridView.DataSource = null;
                CurrentCoursesGridView.DataBind();
            }

        }

        //Bind data for previous classes dropdowns
        protected void ClassesRepeater_BindRepeater()
        {
            List<Course> courses = GrouperMethods.GetCourses().Where(x => x.CoreCourseFlag == true).ToList();

            ClassesRepeater.DataSource = courses;
            ClassesRepeater.DataBind();
        }

        //Bind data for role interest dropdowns
        protected void RolesRepeater_BindRepeater()
        {
            List<Role> roles = GrouperMethods.GetRoles();

            RolesRepeater.DataSource = roles;
            RolesRepeater.DataBind();

        }

        //Bind data for Language skill dropdowns
        protected void LanguagesRepeater_BindRepeater()
        {
            List<ProgrammingLanguage> languages = GrouperMethods.GetLanguages();

            LanguagesRepeater.DataSource = languages;
            LanguagesRepeater.DataBind();
        }

        //Bind data for personal skill dropdowns
        protected void SkillsRepeater_BindRepeater()
        {
            List<Skill> skills = GrouperMethods.GetSkills();

            SkillsRepeater.DataSource = skills;
            SkillsRepeater.DataBind();
        }

        //Function triggered when submit button clicked
        protected void SubmitLinkButton_Click(object sender, EventArgs e)
        {
            Student student = ThisStudent;

            //Set submit time
            student.SurveySubmittedDate = DateTime.Now;

            //Set prefered name
            student.PreferredName = PreferedNameTextBox.Text;

            //Set UOID

            int uoid;
            if(int.TryParse(UOIDTextBox.Text.Trim(), out uoid))
            {
                student.UOID = uoid;
            }

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

                if (courseGradeDropDownList.SelectedIndex > 0)
                {
                    int grade = int.Parse(courseGradeDropDownList.SelectedValue);
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

            //Get dev. experience!
            student.DevelopmentExperience = DevelopmentExperienceTextBox.Text;

            //Get learning expectations
            student.LearningExpectations = LearningExpectationsTextBox.Text;

            //Update Database
            GrouperMethods.UpdateStudent(student);

            //Redirect to Thanks page
            string idString = "?id=" + ThisStudent.StudentID.ToString();
            Response.Redirect("ThankYou.aspx" + idString);
        }

        //Function triggered when student selects different concurrent class from dropdown
        protected void CurrentCoursesDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CurrentCoursesDropDownList.SelectedIndex > 0)
            {
                //Figure out which course
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

                //Add selected course to gridview
                ((List<Course>)ViewState["CurrentCourses"]).Add(course);

                //Rebind
                CurrentCoursesGridView.DataSource = (List<Course>)ViewState["CurrentCourses"];
                CurrentCoursesGridView.DataBind();
            }
        }

        //Function triggered when student changes selection for "English is Primary Language"
        protected void SecondLangDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SecondLanguageDropDownList.SelectedIndex > 0) // They are indicating english is NOT their primary language
            {
                //Prompt for primary langauge
                SecondLangTextBoxLabel.Text = "Please enter your primary language:";
                SecondLangTextBoxLabel.Visible = true;
                SecondLangTextBox.Visible = true;

            }
            else //They are indicating english IS their primary language
            {
                //Hide primary language prompt
                SecondLangTextBoxLabel.Visible = false;
                SecondLangTextBox.Visible = false;
            }
        }

        //Function triggered when studen de-selects a current course
        protected void CurrentCoursesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delete_course")
            {
                //get course
                int courseID = int.Parse(e.CommandArgument.ToString());

                //remvove the course from the view
                if (ViewState["CurrentCourses"] != null)
                {
                    ((List<Course>)ViewState["CurrentCourses"]).RemoveAll(x => x.CourseID == courseID);

                    //Rebind
                    CurrentCoursesGridView.DataSource = (List<Course>)ViewState["CurrentCourses"];
                    CurrentCoursesGridView.DataBind();
                }
            }
        }
    }
}