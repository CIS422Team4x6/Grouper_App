﻿<%@ Page Title="Team Formation Survey" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Oops.aspx.cs" Inherits="GrouperApp.Oops" %>

<%-- The student is redirected to this page if they attempt to re-submit their survey. --%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h2>Oops!</h2>
        <h4>Your information has already been submitted!</h4>
    </div>
</asp:Content>

