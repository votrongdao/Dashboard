﻿multiLoanModule.directive('fixedHeader', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        link: function ($scope, $elem, $attrs, $ctrl) {
            // wait for content to load into table and the tbody to be visible
            $scope.$watch(function () { return $elem.find("tbody").is(':visible') },
                function (newValue, oldValue) {
                    if (newValue === true) {
                        // reset display styles so column widths are correct when measured below
                        $elem.find('thead, tbody').css('display', '');

                        // wrap in $timeout to give table a chance to finish rendering
                        $timeout(function () {
                            // set widths of columns
                            $elem.find('th').each(function (i, thElem) {
                                thElem = $(thElem);
                                tdElems = $elem.find('tbody tr:first td:nth-child(' + (i + 1) + ')');

                                var columnWidth = tdElems.width();
                                thElem.width(columnWidth);
                                tdElems.width(columnWidth);
                            });

                            // set css styles on thead and tbody
                            $elem.find('thead').css({
                                'display': 'block',
                            });

                            $elem.find('tbody').css({
                                'display': 'block',
                                'height': '400px',
                                'overflow': 'auto'
                            });

                            // reduce width of last column by width of scrollbar
                            var scrollBarWidth = $elem.find('thead').width() - $elem.find('tbody')[0].clientWidth;
                            if (scrollBarWidth > 0) {
                                // for some reason trimming the width by 2px lines everything up better
                                scrollBarWidth -= 2;
                                $elem.find('tbody tr:first td:last-child').each(function (i, elem) {
                                    $(elem).width($(elem).width() - scrollBarWidth);
                                });
                            }
                        },500);
                    }
                });
        }
    }
}]);