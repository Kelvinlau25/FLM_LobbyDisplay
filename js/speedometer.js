
    
    function run() {
    
  var a= document.getElementById("txtgaugetoday").value;
 
        var svg = d3.select("#speedometer")
                .append("svg:svg")
                .attr("width", 150)
                .attr("height", 150);


        var gauge = iopctrl.arcslider()
                .radius(65)
                .events(false)
                .indicator(iopctrl.defaultGaugeIndicator);
        gauge.axis().orient("out")
                .normalize(true)
                .ticks(11)
                .tickSubdivide(3)
                .tickSize(10, 8, 10)
                .tickPadding(5)
                .scale(d3.scale.linear()
                        .domain([0, 100])
                        .range([-1*Math.PI/2, 1*Math.PI/2]));

        var segDisplay = iopctrl.segdisplay()
                .width(70)
                .digitCount(3)
                .negative(false)
                .decimals(0);
       
      

if (a>=0){

        svg.append("g")
                .attr("class", "segdisplay")
                .attr("transform", "translate(130, 200)")
                .call(segDisplay);
                
        svg.append("g")
                .attr("class", "gauge")
                .call(gauge);
                
}
else{
 a=a.substring(1);
 
 svg.append("g")
                .attr("class", "segdisplay1")
                .attr("transform", "translate(130, 200)")
                .call(segDisplay);
                
   svg.append("g")
                .attr("class", "gauge1")
                .call(gauge);
}
        segDisplay.value(a);
        gauge.value(a);
        
        
        if (document.getElementById("txtgaugeacc")){
        
         var a= document.getElementById("txtgaugeacc").value;
         var svg = d3.select("#speedometer2")
                .append("svg:svg")
                .attr("width", 150)
                .attr("height", 150);


        var gauge = iopctrl.arcslider()
                .radius(65)
                .events(false)
                .indicator(iopctrl.defaultGaugeIndicator);
        gauge.axis().orient("out")
                .normalize(true)
                .ticks(11)
                .tickSubdivide(3)
                .tickSize(10, 8, 10)
                .tickPadding(5)
                .scale(d3.scale.linear()
                        .domain([0, 100])
                        .range([-1*Math.PI/2, 1*Math.PI/2]));

        var segDisplay = iopctrl.segdisplay()
                .width(70)
                .digitCount(3)
                .negative(false)
                .decimals(0);
       
      

if (a>=0){

        svg.append("g")
                .attr("class", "segdisplay")
                .attr("transform", "translate(130, 200)")
                .call(segDisplay);
                
        svg.append("g")
                .attr("class", "gauge")
                .call(gauge);
                
}
else{
 a=a.substring(1);
 svg.append("g")
                .attr("class", "segdisplay1")
                .attr("transform", "translate(130, 200)")
                .call(segDisplay);
                
   svg.append("g")
                .attr("class", "gauge1")
                .call(gauge);
}
        segDisplay.value(a);
        gauge.value(a);
    
    }
    
  }
     
 