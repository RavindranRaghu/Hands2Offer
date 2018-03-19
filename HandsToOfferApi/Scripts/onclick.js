$(function(){
    // on click settings
    $(".navbar-brand").click(function(){
        $("#bodyhtml").load("home.html");
    });
    $(".upcomingeventLink").click(function(){
        $("#bodyhtml").load("upcoming.html");
    });
    $(".projectLink").click(function(){
        $("#bodyhtml").load("project.html");
    });
    $(".recenteventLink").click(function(){
        $("#bodyhtml").load("recent.html");
    });
});