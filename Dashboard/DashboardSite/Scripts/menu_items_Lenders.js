// items structure
// each item is the array of one or more properties:
// [text, link, settings, subitems ...]
// use the builder to export errors free structure if you experience problems with the syntax

        var MENU_ITEMS = [
	        ['Home', null, null,
		        ['Contact Us', '/Home/Contact'],
                ['News', '/Rss/Index'],
		        ['Log Out', '/Account/LogOff'],
		    ],
	        ['Financial Analysis', null, null,
		        ['Upload One File', '/ExcelUpload/ExcelUploadView'],
		        ['Excel Upload Report', '/ExcelUploadView_/ReportExcelUpload'],
                ['Upload Status Report', '/ExcelUploadView_/UploadStatusReport'],
                ['Risk Score Report', '/ExcelUploadView_/ViewExcelUploadPageSort'],
	        ],
	        ['Asset Management', null, null,
		        ['Reports 1', 'CMMIPALList.aspx'],
		        ['Reports 2', 'Wizard_Instructions.aspx'],
		        ['Reports 3', 'Wizard_Template.aspx'],
		        ['Reports 4', 'Wizard_Application.aspx'],
	        ]
        ];



