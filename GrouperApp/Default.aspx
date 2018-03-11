<%@ Page Title="Team Formation Survey" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GrouperApp._Default" %>

<%-- Survey main page: contains all necessary questions for group formation --%>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   
    <%-- Title Heading --%>
    <h1>Welcome <asp:Label ID="FirstNameLabel" runat="server" />&nbsp;<asp:Label ID="LastNameLabel" runat="server" />!</h1>
    <h3>Please fill out the following survey. Your answers will be used to assign groups.</h3>
    </br>

    <%-- This sumarises any validation error that have occured when the user tries to submit --%>
    <asp:ValidationSummary ID="GeneralValidationSummary" runat="server" HeaderText="<h4>Errors were found in your submission.  Please correct.</h4>" CssClass="text-danger" ShowMessageBox="false" />

    <h4>General</h4>
    <%-- This section collects prefered name, UOID and primary language --%>

    <div class='panel panel-default'>
        <div class="container">
            <div class="row">
                <div class="col-md-3">
                    <div class="form-group">
                        <asp:Label CssClass="control-label" runat="server">I prefer to be called: </asp:Label>
                        <asp:TextBox ID="PreferedNameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-3">
                    <div class="form-group">
                        <asp:Label runat="server" CssClass="control-label">UO Student ID: <span class="text-muted">(95*******)</span></asp:Label>
                        <asp:TextBox ID="UOIDTextBox" runat="server" CssClass="form-control" MaxLength="9"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="UOIDRegExValidator" runat="server" ControlToValidate="UOIDTextBox" ErrorMessage="Invalid UO Student ID" CssClass="text-danger" ValidationExpression="[9][5]\d{7}"></asp:RegularExpressionValidator>
                        <asp:RequiredFieldValidator ID="UOIDRequiredFieldValidator" runat="server" ControlToValidate="UOIDTextBox" ErrorMessage="UO Student ID required field" CssClass="text-danger"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-3">
                    <asp:Label ID="SecondLangDropDownLabel" runat="server" Text="Is English your primary language?" CssClass="control-label"></asp:Label>
                    <asp:DropDownList ID="SecondLanguageDropDownList" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="SecondLangDropDownList_SelectedIndexChanged">
                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-6">
                    <asp:Label ID="SecondLangTextBoxLabel" runat="server" CssClass="control-label" Visable="false"></asp:Label>
                    <asp:TextBox ID="SecondLangTextBox" runat="server" Visible="false" MaxLength="100" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <br />
    <h4>CIS Course Experience</h4>
    <%-- This section collects concurrent CIS courses and grades from previously taken courses --%>

    <div class='panel panel-default'>
    <div class="row">
        <div class="col-md-3">
            <asp:Label CssClass="control-label" runat="server">Select Current Courses: </asp:Label>
            <asp:DropDownList ID="CurrentCoursesDropDownList" runat="server" CssClass="form-control" DataTextField="FullName" DataValueField="CourseID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="CurrentCoursesDropDownList_SelectedIndexChanged">
                <asp:ListItem Text="Select..." Value=""></asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="col-md-6">
            <asp:Label CssClass="control-label" runat="server">Your Current Courses:</asp:Label>
            <asp:GridView ID="CurrentCoursesGridView" runat="server" DataKeyNames="CourseID" CssClass="table table-bordered table-striped table-condensed" AutoGenerateColumns="false" ShowHeader="false" OnRowCommand="CurrentCoursesGridView_RowCommand">
                <EmptyDataTemplate>No current courses selected</EmptyDataTemplate>
                <Columns>
                    <asp:BoundField DataField="FullName" />
                    <asp:TemplateField ItemStyle-Width="1%">
                        <ItemTemplate>
                            <asp:LinkButton ID="RemoveCourseLinkButton" runat="server" CssClass="btn-danger btn-xs" CommandName="delete_course" CommandArgument='<%# Eval("CourseID") %>'><span class="glyphicon glyphicon-remove"></span></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    

    <br />

    <asp:Repeater ID="ClassesRepeater" runat="server">
        <HeaderTemplate>
            <div class="row">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-3">
                <div class="form-group">
                    <asp:Label ID="CourseNameLabel" runat="server" Text='<%# Eval("Code") %>' CssClass="control-label"></asp:Label>
                    <asp:HiddenField ID="CourseIDHiddenField" runat="server" Value='<%# Eval("CourseID") %>' />
                    <asp:DropDownList ID="GradeDropDownList" runat="server" CssClass="form-control input-sm">
                        <asp:ListItem Text="Not Taken" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="A+" Value="4.3"></asp:ListItem>
                        <asp:ListItem Text="A" Value="4"></asp:ListItem>
                        <asp:ListItem Text="A-" Value="3.7"></asp:ListItem>
                        <asp:ListItem Text="B+" Value="3.3"></asp:ListItem>
                        <asp:ListItem Text="B" Value="3"></asp:ListItem>
                        <asp:ListItem Text="B-" Value="2.7"></asp:ListItem>
                        <asp:ListItem Text="C+" Value="2.3"></asp:ListItem>
                        <asp:ListItem Text="C" Value="2"></asp:ListItem>
                        <asp:ListItem Text="C-" Value="1.7"></asp:ListItem>
                        <asp:ListItem Text="D+" Value="1.3"></asp:ListItem>
                        <asp:ListItem Text="D" Value="1"></asp:ListItem>
                        <asp:ListItem Text="D-" Value="0.7"></asp:ListItem>
                        <asp:ListItem Text="F" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    </div>

    <br />
    <h4>Role Interests</h4>
    <%-- This section collects information about how interested the student is in different roles --%>

    <div class='panel panel-default'>
    <asp:Repeater ID="RolesRepeater" runat="server">
        <HeaderTemplate>
            <div class="row">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-3">
                <div class="form-group">
                    <asp:Label ID="RoleNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="control-label"></asp:Label>
                    <asp:HiddenField ID="RoleIDHiddenField" runat="server" Value='<%# Eval("RoleID") %>' />
                    <asp:DropDownList ID="InterestDropDownList" runat="server" CssClass="form-control input-sm">
                        <asp:ListItem Text="Not Interested" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Not Very Interested" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Indifferent" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Somewhat Interested" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Very Interested" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    </div>

    <br />
    <h4>Programming Languages</h4>
    <%-- This section collects information about the students experience in different languages --%>

    <div class='panel panel-default'>
    <asp:Repeater ID="LanguagesRepeater" runat="server">
        <HeaderTemplate>
            <div class="row">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-3">
                <div class="form-group">
                    <asp:Label ID="LanguageNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="control-label"></asp:Label>
                    <asp:HiddenField ID="LanguageIDHiddenField" runat="server" Value='<%# Eval("LanguageID") %>' />
                    <asp:DropDownList ID="LanguageDropDownList" runat="server" CssClass="form-control input-sm">
                        <asp:ListItem Text="No Experience" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Somewhat Experienced" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Very Experienced" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    </div>

    <br />
    <h4>Skills</h4>
    <%-- This section collects information about certain interpersonall/group-oriented skills --%>

    <div class='panel panel-default'>
    <asp:Repeater ID="SkillsRepeater" runat="server">
        <HeaderTemplate>
            <div class="row">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-3">
                <div class="form-group">
                    <asp:Label ID="SkillNameLabel" runat="server" Text='<%# Eval("Name") %>' CssClass="control-label"></asp:Label>
                    <asp:HiddenField ID="SkillIDHiddenField" runat="server" Value='<%# Eval("SkillID") %>' />
                    <asp:DropDownList ID="SkillDropDownList" runat="server" CssClass="form-control input-sm">
                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
    </div>

    <br />
    <h4>Free Response</h4>
    <%-- This section consists of two text areas for collecting Professional experience and Learning expectations --%>

    <div class='panel panel-default'>
    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <asp:Label CssClass="control-label" runat="server">Professional Development Experience (if any):</asp:Label>
                <asp:Label CssClass="text-muted small" runat="server">(max 5000 characters)</asp:Label>
                <asp:TextBox ID="DevelopmentExperienceTextBox" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="DevelopmentExperienceTextBox" ErrorMessage="5000 character limit!" CssClass="text-danger" ValidationExpression="^[\s\S]{0,5000}$"></asp:RegularExpressionValidator>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <asp:Label CssClass="control-label" runat="server">Briefly describe what you expect to learn in this class: </asp:Label>
                <asp:Label CssClass="text-muted small" runat="server">(max 5000 characters)</asp:Label>
                <asp:TextBox ID="LearningExpectationsTextBox" runat="server" maxLength="10" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="LearningExpectationsTextBox" ErrorMessage="5000 character limit!" CssClass="text-danger" ValidationExpression="^[\s\S]{0,5000}$"></asp:RegularExpressionValidator>
            </div>
        </div>
    </div>

    </div>

    <div class="row">
        <div class="col-md-3">
            <asp:LinkButton ID="SubmitLinkButton" runat="server" CssClass="btn btn-default" OnClick="SubmitLinkButton_Click">Submit</asp:LinkButton>
        </div>
    </div>

    
</asp:Content>
