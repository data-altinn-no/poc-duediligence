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
      for (let i=0; i<Math.max(0, industryCode.level - 2); i++) {
          ret += "\xa0\xa0\xa0\xa0";
      }
      return ret + "(" + industryCode.code + ") " + industryCode.name;;
  }

  function performSearch(evt, offsetPage = 0, append = false, onComplete = null) {
      
      if (!append) {
        $('#results').html($('#spinner').html());
        $('#performSearch').attr('disabled', 'disabled');
      }

      var orgno = document.querySelector('input').value;
      var re = new RegExp("^([0-9]{4}:)?([0-9]{9})$");

      if (!re.test(orgno.trim())) {
          $('#industry-codes').css('background-color', "red");
          $('#results').html("<h3 class='text-center'>Ugyldig organisasjonsnummer.</h3>");
          $('#performSearch').removeAttr('disabled');
          return;
      } else {
          $('#industry-codes').css('background-color', "white");
      }

      var query = { OrganisationNumber: orgno, Append : false }
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
              if (append) {
                var prevRows = $('#results').data('total-rows');
                
                $('#results').find ('tbody').append(data);
                $('#results').data('total-rows', $('#results tbody').find('tr').length);
                if (data.trim() == "" || $('#results').data('total-rows') - prevRows < 20) {
                  $('#fetch-more').hide();
                }
                else {
                  $('#fetch-more').data('offset-page', $('#fetch-more').data('offset-page') + 1);
                }

                $('#performSearch').removeAttr('disabled');
              }
              else {
                $('#results').html(data);
                $('#results').data('total-rows', $('#results tbody').find('tr').length);
                $('#fetch-more').data('offset-page', 1);
                if ($('#results').data('total-rows') < 20) {
                  $('#fetch-more').hide();
                }
              }
              
              $('#performSearch').removeAttr('disabled');
              $('#result-list').tablesort({ compare: compare });

              if (typeof onComplete == "function") onComplete();
          },
          error: function(err) {
              $('#results').html("<h3 class='text-center'>Beklager, det oppstod en feil.</h3>");
              $('#performSearch').removeAttr('disabled');
              if (typeof onComplete == "function") onComplete();
          }
      });
  }

  function compare(a, b){
    if (typeof a == "string") a = a.trim(); 
    if (typeof b == "string") b = b.trim();
    if (!isNaN(a)) a *= 1; if (!isNaN(b)) b *= 1;
    if (a > b) {
      return 1;
    } else if (a < b) {
      return -1;
    }
    return 0;
  }

  $.widget( "app.autocomplete", $.ui.autocomplete, {
      
    // Which class get's applied to matched text in the menu items.
    options: {
        highlightClass: "ui-state-highlight"
    },
    
    _renderItem: function( ul, item ) {
        var re = new RegExp( "(" + this.term + ")", "gi" ),
            cls = this.options.highlightClass,
            template = "<span class='" + cls + "'>$1</span>",
            label = item.label.replace( re, template ),
            $li = $( "<li/>" ).appendTo( ul );
        
        // Create and return the custom menu item content.
        $( "<div/>" ).html( label )
                    .appendTo( $li );
        
        return $li;
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
      search: function() {
        if ( this.value.length < 2 ) {
          return false;
        }
      },
      response: function( event, ui ) {
        var term = $(event.target).val();
        var strongTerm = "<strong>" + term + "</strong>";
        ui.content.forEach((x) => {
          x.value = x.value.replace(term, strongTerm)
        });
      },
      select: function( event, ui ) {
        this.value = "";  
        if ($('#selected-industry-codes').find('[data-value="' + ui.item.name + '"]').length != 0) return false;
        $('#selected-industry-codes').append('<div class="selected-term selected-term-industrycode" data-value="' + ui.item.name + '">' + ui.item.value.trim() + '<a href="javascript:" class="close"></a></div>');
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
          if ($('#selected-municipalities').find('[data-value="' + ui.item.name + '"]').length != 0) return false;
          $('#selected-municipalities').append('<div class="selected-term selected-term-municipality" data-value="' + ui.item.name + '">' + ui.item.value.trim() + '<a href="javascript:" class="close"></a></div>');
          return false;
        }
    });


    $('#performSearch').on('click', performSearch);

    $('.selected-terms').on('click', '.selected-term a', function(e) {
      $(this).parent().remove();
    });

    $('#results').on('click', '#fetch-more', function() {
      var $self = $(this);
      var offsetPage = $self.data('offset-page');
      $self.attr('disabled', 'disabled');
      $self.data('orig', $self.text());
      $self.text('Vennligst vent ...');
      performSearch(null, offsetPage, true, function() {
        $self.removeAttr('disabled');
        $self.text($self.data('orig'));  
      });
    })

});