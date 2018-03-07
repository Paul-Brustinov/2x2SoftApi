<template>
  <div id='MyWorkTimes'>
      <button v-if="!showLogin" v-on:click="showLoginForm()" class="btn js-add btn-default">Login</button>
      <loginForm v-if="showLogin" v-on:close="showLogin = false">
      </loginForm>
      <h2>My WorkTimes</h2>
      <div id="work-times-list">
          <p>
              <button v-on:click="edit(0)" class="btn btn-default">Create new work time</button>

            <input inputmode="" type="text" placeholder="input filter" v-model="filter1"/>
            Страница <input inputmode="" type="text" placeholder="page" v-model="pageNo"/>

            <button v-on:click="prevPage" class="btn btn-default">&lt;</button>
            <button v-on:click="nextPage" class="btn btn-default">></button>

            <!-- use the modal component, pass in the prop -->
            показано {{(pageNo-1) * pageRows + 1}} - {{pageNo * pageRows}}
            из {{rows}}
            страниц {{pages}}
          </p>
          <table>
              <tr>
                  <th  v-on:click="sortBy('OpDate')"  :class="{active: sortKey == 'OpDate' }" class="pointer">
                     Date1
                      <span class="arrow" :class="sortOrders['OpDate'] > 0 ? 'asc' : 'dsc'"> </span>
                  </th>
                  <th  v-on:click="sortBy('ClientName')" :class="{active: sortKey == 'ClientName' }"  class="pointer">
                      Client
                      <span class="arrow" :class="sortOrders['ClientName'] > 0 ? 'asc' : 'dsc'"> </span>
                  </th>
                  <th>
                    First details
                  </th>
                  <th>
                    Actions
                  </th>
              </tr>

              <tr v-for="item in filtered_items" :key="item.Id">
                  <td>{{item.Date}}</td>
                  <td>{{item.ClientName}}</td>
                  <td>{{item.Name}}</td>
                  <td>
                      <span class="input-group-btn">
                          <button v-on:click="edit(item.Id)" class="btn js-add  btn-default">Edit</button>
                          <button v-on:click="deleteWorkTime(item.Id)" class="btn js-add btn-default">Delete</button>
                      </span>
                  </td>
              </tr>
          </table>

          <workTime v-if="showModal" v-on:close="showModal = false" :StartUrl="StartUrl" :WorkTimes="workTimes" :WorkTime="currentWorkTime" :Persons="persons">
          </workTime>
      </div>

</div>


  <!--</div>-->
</template>

<script>

import workTime from './WorkTime.vue'
import loginForm from './LoginForm.vue'


export default {
        props: ['StartUrl'],
        data:function () {
          var sortOrders = {};
          sortOrders['OpDate'] = -1;
          sortOrders['ClientName'] = 1;

          var MyWorkTimes = null;
          var Email = '';
          var Persons = null;
          var json;

          MyWorkTimes = [{Date: '', ClientName: '', Name: ''}]
          Persons = [{Id: 0, Name: 'Брустинов'}]

          this.load();

          return {
            workTimes: [{Date: '', ClientName: '', Name: ''}],
            filter: "",
            pageNo: 1,
            pageRows: 50,
            pages: 0,
            rows: 0,
            showModal: false,
            showLogin: true,
            token: "",
            currentId: 0,
            persons: [{Id: 0, Name: ''}],
            currentWorkTime: {},

            sortKey: '',
            sortOrders: sortOrders
          }
        },
        components: {
          workTime,
          loginForm
        },
        computed: {
          filtered_items: function() {
            var ret = [];
            var ret0 = [];
            var self = this;

            var sortKey = this.sortKey;
            var order = this.sortOrders[sortKey] || 1;

            ret0 = self.workTimes.slice();

            if (sortKey) {
              ret0 = self.workTimes.slice().sort(function(a, b) {
                return (a[sortKey] === b[sortKey] ? (
                  (a['OpDate'] === b['OpDate'] ? (
                    (a['ClientName'] === b['ClientName'] ? 0 : a['ClientName'] > b['ClientName'] ? 1 : -1) * self.sortOrders['ClientName'] || 1
                  ) : a['OpDate'] > b['OpDate'] ? 1 : -1) * self.sortOrders['OpDate'] || 1
                ) : a[sortKey] > b[sortKey] ? 1 : -1) * order;
              });
            }

            ret0 = ret0.filter(function(item) {
              return item.ClientName.toLowerCase().indexOf(self.filter.toLowerCase()) !== -1;
            });
            ret = ret0.filter(function(item, index) {
              return (index >= (0 + (self.pageNo - 1) * self.pageRows) && index < (self.pageNo) * self.pageRows);
            });

            this.pages = Math.ceil(ret0.length / this.pageRows);
            self.rows = ret0.length;
            return ret;
          },
          filter1: {
            get: function() {
              return this.filter;
            },
            set: function(newValue) {
              this.pageNo = 1;
              this.filter = newValue;
            }
          }
        },
        methods: {
          showLoginForm: function(){this.showLogin = true;},
          nextPage: function(event) {
            if (this.pageNo < this.pages) this.pageNo++;
          },
          prevPage: function(event) {
            if (this.pageNo > 1) this.pageNo--;
          },
          sortBy: function (key) {
            this.sortKey = key;
            this.sortOrders[key] = this.sortOrders[key] * -1;
          },
          load: function() {
            var self = this;

            //var token = "Bearer AZz9wA1vPU10mIJAmNbKDSxIS6Y_cZc2GQpYis7iE0u1u-o6K1NySsRVgbRY0FWLjmm0SVxcKnQEzvi6TzDSkbPRnQOF0up9yfKhNSawcGgdbOVpZL3o2rdmwAEHO2-Z-_VzuDmNW4su9VL0ko70_YCS_F6UkLzGG9RCZdteKAohPugHqg4glAKyMSPBQdBri7jSEuXAhDd1r-CKprvqK_g4QliOGZymlCkfmAfGcA56LTWBcxb0yboDxYvt7jShnpestz_6-NngASHqKaDzOOu7cLOgnrni0fwl5zB30H7tznYRItLzkcCX9d3TbQhxUOu9kHhXbn9G-2iYySiR8nNptgwz6_zhkcs3gr2xw7dxJ7xhcuboy9MfNJE4pRUQ4w-5v2tpcGTZGTwd9TUE3f4-KrY5UrXZ45BpBeJavxjHQF73X5fcvYi5aqTjwqAb_qp9AKh-dFbRKjhYqG7shUJHHesRO1gQBJI-OUQKyZU";
            var token = "Bearer " + sessionStorage.getItem("tokenKey");

            var xhttp = new XMLHttpRequest();
            xhttp.open("GET", "http://localhost:53973/api/WorkTime/MyWorkTimes", true);
            xhttp.setRequestHeader("Authorization", token);
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    //console.log(this.responseText);
                    if (this.responseText != "") {
                        var a = JSON.parse(this.responseText);

                       self.workTimes  = a.WorkTimes;
                       self.persons    = a.Persons;

                        console.log(a.Email);
                    }
                }
            };
            xhttp.send();
          },
          deleteWorkTime: function(i) {
            var result = confirm("Want to delete?");
            if (!result) return;
            var xhr = new XMLHttpRequest();
            xhr.open("GET", "Delete/"+i, true);
            xhr.onreadystatechange = () => {
                if (xhr.readyState === 4 && xhr.status === 200) {
                  var removeIndex = this.workTimes.map(function(item) { return item.Id; }).indexOf(Number(i));
                  ~removeIndex && this.workTimes.splice(removeIndex, 1);
                }
            };
            xhr.send();
          },
          edit: function(i) {

            if (i !== 0) {
              this.currentId = i;
              this.currentWorkTime = this.workTimes.find(el => el.Id === i);

            var token = "Bearer " + sessionStorage.getItem("tokenKey");

            var xhttp = new XMLHttpRequest();
            console.log(i);
            xhttp.open("GET", "http://localhost:53973/api/WorkTime/AddOrUpdate", true);
            xhttp.setRequestHeader("Authorization", token);
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    //console.log(this.responseText);
                    if (this.responseText != "") {
                        // var data = JSON.parse(this.responseText);

                        // this.currentWorkTime.WorkTimeDetails = data.WorkTimeDetails;
                        // this.currentWorkTime.WorkTimeDetails.push({ Description: "", Hours: 0.00 });
                        // this.currentWorkTime.Client = this.persons.find((el) => el.Id === data.ClientId);
                        // this.currentWorkTime.Supporter = this.persons.find((el) => el.Id === data.SuppId);
                    }
                }
            };
            xhttp.send();

            } else {
              this.currentId = 0;
              var tmpWorkTime = null;
              if (this.workTimes.length !== 0) {
                tmpWorkTime = this.workTimes.reduce((prev, current) => (prev.OpDate > current.OpDate) ? prev : current);
              }

              var suppId = 0;
              var supporter = null;
              var clientId    = 0;
              var client      = null;
              var supporterName = "";
              var clientName = "";

              if (tmpWorkTime !== null && tmpWorkTime !== undefined) {
                suppId    = tmpWorkTime.SuppId;
                supporter = tmpWorkTime.Supporter;
                supporterName = supporter!==null&&supporter!==undefined?supporter.Name:"";

                clientId    = tmpWorkTime.СlientId;
                client      = tmpWorkTime.Client;
                clientName = client!==null&&client!==undefined?client.Name:"";
              }

              this.currentWorkTime = {
                Id:0 
                ,OpDate:new Date()
                ,ClientId:clientId
                ,SuppId:suppId
                ,WorkTimeDetails:[{Id:0, Description:""}]
                ,Client:client
                ,Supporter:supporter
                ,ClentName:clientName
                ,SupporterName:supporterName
                ,Date:("0"+new Date().getDate()).slice(-2) +"/"+ ("0"+(new Date().getMonth() + 1)).slice(-2) + "/" + new Date().getFullYear()
                ,Name:""
              }
            }
            this.showModal = true;
          }
        }

}
</script>

<style scoped>
    th {
    background-color: #42b983;
    color: rgba(255,255,255,0.66);
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    }

    .pointer {
    cursor: pointer;
    }


    td {
    background-color: #f9f9f9;
    }

    th, td {
    min-width: 120px;
    padding: 10px 20px;
    }

    td{
      text-align:left;
    }

    th.active {
    color: #fff;
    }

    th.active .arrow {
    opacity: 1;
    }

    .arrow {
    display: inline-block;
    vertical-align: middle;
    width: 0;
    height: 0;
    margin-left: 5px;
    opacity: 0.66;
    }

    .arrow.asc {
    border-left: 4px solid transparent;
    border-right: 4px solid transparent;
    border-bottom: 4px solid #fff;
    }

    .arrow.dsc {
    border-left: 4px solid transparent;
    border-right: 4px solid transparent;
    border-top: 4px solid #fff;
    }

</style>
