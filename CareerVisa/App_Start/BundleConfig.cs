﻿using System.Web;
using System.Web.Optimization;

namespace CareerVisa
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.maskedinput.min.js",
                        "~/Scripts/jquery.backstretch.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalR").Include(
                "~/Scripts/jquery.signalR-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/wow.js",
                      "~/Scripts/main.js",
                      "~/Scripts/gridmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/main.css",
                      "~/responsive.css",
                      "~/fonts/font-awesome/font-awesome.min.css",
                      "~/Content/extras/animate.css",
                      "~/Content/style.css",
                      "~/Content/form-elements.css",
                      "~/font-awesome/css/font-awesome.min.css",
                      "~/Content/Gridmvc.css"));


            bundles.Add(new ScriptBundle("~/bundles/scriptsRegistration").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/wow.js",
                    "~/Scripts/main.js",
                    "~/Scripts/scripts.js",
                    "~/Scripts/gridmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/cssRegistration").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/main.css",
                      "~/responsive.css",
                      "~/fonts/font-awesome/font-awesome.min.css",
                      "~/Content/extras/animate.css",
                      "~/Content/style.css",
                      "~/Content/form-elements.css",
                      "~/font-awesome/css/font-awesome.min.css",
                      "~/Content/Gridmvc.css",
                     "~/Content/style.css",
                     "~/Content/form-elements.css",
                     "~/font-awesome/css/font-awesome.min.css",
                     "~/Content/Gridmvc.css"));


            bundles.Add(new ScriptBundle("~/bundles/scriptsdashboard").Include(
                      //"~/Scripts/jquery.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      //"~/Scripts/bootstrap.min.js",
                      "~/Scripts/wow.js",
                      "~/Scripts/plugins/morris/raphael.min.js",
                      "~/Scripts/gridmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/cssdashboard").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/sb-admin.css",
                      "~/Content/morris.css",
                      "~/font-awesome/css/font-awesome.min.css",
                      "~/Content/custom.min.css",
                      "~/Content/Gridmvc.css"));
        }
    }
}
