// items structure
// each item is the array of one or more properties:
// [text, link, settings, subitems ...]
// use the builder to export errors free structure if you experience problems with the syntax

        var MENU_ITEMS = [
	        ['Home', null, null,
		        ['Contact Us', '/Home/Contact'],
		        ['Log Out', '/Account/LogOff'],
		    ],
	        ['Financial Analysis', null, null,
		        ['Reports', '/ExcelUploadView_/ViewExcelUpload'],
                ['Dashboard', '/ExcelUploadView_/ViewExcelUpload'],
	        ],
	        ['Asset Management', null, null,
		        ['Read Reports 1', '/ExcelUploadView_/ViewExcelUpload'],
		        ['Read Reports 2', '/ExcelUploadView_/ViewExcelUpload']
	        ],
            ['Production', null, null,
	            ['Production 1', '/ExcelUploadView_/ViewExcelUpload'],
	            ['Production 2', '/ExcelUploadView_/ViewExcelUpload']
            ]
        ];



