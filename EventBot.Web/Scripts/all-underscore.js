<ul class="notifications">

           <%
           _.each(notifications, function(notification){
           if (notification.type == 1) { %>
           <li><span class="highlight"><%= notification.event.organiser.name %></span> har avbokat aktiviten som var planerad <%= moment(notification.event.startDate).format("D MMM HH:mm") %>.</li>
           <% }

else if (notification.type == 2) {
           var changes = [],
           originalValues = [],
           newValues = [];

           if (notification.originalStartDate != notification.event.startDate) {
           changes.push('datum/tid');
           originalValues.push(notification.originalStartDate);
           newValues.push(notification.event.startDate);
           %>
           <li><span class="highlight"><%= notification.event.organiser.name %></span> har ändrat <%= changes.join(' and ') %> från <%= originalValues.join('/') %> till <%= newValues.join('/') %></li>
           <%}

else {%>
           <li><span class="highlight"><%= notification.event.organiser.name %></span> har uppdaterat detaljerna i eventet som är planerat <%= notification.event.startDate %>.</li>
           <%}
}
})
           %>
       </ul>