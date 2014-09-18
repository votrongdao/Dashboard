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
                ['Risk Summary', '/Chart/Index'],
                ['Property With Multiple Loans', '/Reports/MultipleLoansProperty'],
                ['Property With Multiple Loans2', '/MultiLoan/Index'],
	        ],
	        ['Asset Management', null, null,
		        ['Reports 1', 'CMMIPALList.aspx'],
		        ['Reports 2', 'Wizard_Instructions.aspx'],
		        ['Reports 3', 'Wizard_Template.aspx'],
		        ['Reports 4', 'Wizard_Application.aspx'],
	        ],
            ['Production', null, null,
	            ['Production 1', 'CMMIPIIDWizard.aspx'],
	            ['Production 2', 'CMMIPIIDWizard.aspx']
            ],
	        ['Administration', null, null,
		        ['Manage users', '/Account/ListUsers'],
                ['Register', '/Account/Register'],
                ['Admin 2', 'CMMIAutentificationLogOnHistory.aspx'],
                ['Admin 3', 'CMMIApplicationGroupList.aspx'],
		        ['Admin 4', 'CMMIApplicationIDList.aspx'],
		        ['Admin 5', 'CMMIApplicationAssigningToUserList.aspx'],
		        ['Admin 6', 'CMMIDeletedApplicationTransactionList.aspx']
	        ]  
        ];



