// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

$( function() {
    function split( val ) {
      return val.split( /,\s*/ );
    }
    function extractLast( term ) {
      return split( term ).pop();
    }

    function processIndustryCodes(response) {
        var ret = [];
        response.forEach((r) => {
            ret.push({ "name": r.code, "value": formatIndustryCodeName(r) } );
        });

        return ret;
    }

    function processMunicipalities(response) {
        var ret = [];
        response.forEach((r) => {
            ret.push({ "name": r.kommunenummer, "value": r.kommunenavnNorsk } );
        });
        return ret;
    }

    function formatIndustryCodeName(industryCode) {
        var ret = "";
        for (let i=0; i<Math.max(0, industryCode.level - 3); i++) {
            ret += "\xa0\xa0\xa0\xa0";
        }
        return ret + "(" + industryCode.code + ") " + industryCode.name;;
    }

    $.widget( "ui.autocomplete", jQuery.ui.autocomplete, {
        _close: function( event ) {
            if(event!== undefined && event.keepOpen===true) {
                return true;
            }
            //otherwise invoke the original
            return this._super( event );
        }
    });

    $( "#industry-codes" )
      // don't navigate away from the field on tab when selecting an item
      .on( "keydown", function( event ) {
        if ( event.keyCode === $.ui.keyCode.TAB &&
            $( this ).autocomplete( "instance" ).menu.active ) {
          event.preventDefault();
        }
      })
      .autocomplete({
        source: function( request, response ) {
          $.getJSON( "/api/industrycodes/search/" + extractLast( request.term ), {}, (r) => response(processIndustryCodes(r)));
        },
        focus: function() {
          // prevent value inserted on focus
          return false;
        },
        select: function( event, ui ) {
          this.value = "";  
          $('#selected-industry-codes').append('<div class="selected-term selected-term-industrycode" data-value="' + ui.item.name + '">' + ui.item.value.trim() + '</div>');
          jQuery.extend(event.originalEvent,{keepOpen:true});
          return false;
        }
      });

      $( "#municipalities" )
      // don't navigate away from the field on tab when selecting an item
      .on( "keydown", function( event ) {
        if ( event.keyCode === $.ui.keyCode.TAB &&
            $( this ).autocomplete( "instance" ).menu.active ) {
          event.preventDefault();
        }
      })
      .autocomplete({
        source: function( request, response ) {
          $.getJSON( "/api/municipalities/search/" + extractLast( request.term ), {}, (r) => response(processMunicipalities(r)));
        },
        focus: function() {
          // prevent value inserted on focus
          return false;
        },
        select: function( event, ui ) {
          this.value = "";  
          $('#selected-municipalities').append('<div class="selected-term selected-term-municipality" data-value="' + ui.item.name + '">' + ui.item.value.trim() + '</div>');
          jQuery.extend(event.originalEvent,{keepOpen:true});
          return false;
        }
      });


      $('#performSearch').on('click', function() {
            var query = { industryCodes: [], municipalities: [] };
            $('#selected-industry-codes').find('.selected-term').each((_, el) => query.industryCodes.push($(el).data('value')));
            $('#selected-municipalities').find('.selected-term').each((_, el) => query.municipalities.push($(el).data('value')));

            $.ajax({
                type: "POST",
                beforeSend: function (xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                url: "/Search",
                datType: "html",
                data: query,
                success: function (data) {
                    $('#results').html(data);
                }
            });
      });
  } );