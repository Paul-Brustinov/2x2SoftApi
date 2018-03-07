<template>
    <transition name="modal">
        <div class="modal-mask">
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">
            <div class="modal-wrapper">
                <div class="modal-container">

                    <div class="modal-header">
                        <slot name="header">
                            WorkTime
                        </slot>
                    </div>

                    <div class="modal-body input-group">
                        <slot name="body">

                            <div class="form-group row">
                                <label class="control-label col-md-3" for="OpDate">OpDate</label>
                                <div class="col-md-9">
                                    <input id="Date" name="Date" v-model="WorkTime.Date" class="form-control" />
                                </div>
                            </div>

                            <div class="form-group row">
                                <label class="control-label col-md-3" for="Client">Client</label>
                                <div class="col-md-9">
                                    <vselect :debounce="1000"
                                              :on-search="getClientOptions"
                                              :on-change="getClientIdCallback"
                                              :options="optionsClient"
                                              :value=WorkTime.Client
                                              placeholder="Search client..."
                                              label="Name"
                                              >
                                    </vselect>
                                </div>
                            </div>
                            
                            <div class="form-group row">
                                <label class="control-label col-md-3" for="Client">Supporter</label>
                                <div class="col-md-9">
                                    <vselect :debounce="1000"
                                              :on-search="getSuppOptions"
                                              :on-change="getSuppIdCallback"
                                              :options="optionsSupp"
                                              :value=WorkTime.Supporter
                                              placeholder="Search supporter..."
                                              label="Name"
                                              >
                                    </vselect>
                                </div>
                            </div>

                            <div class="form-group row">
                              <tr v-for="descr in WorkTime.WorkTimeDetails" v-bind:key="descr.Id">
                                  <td>
                                      <textarea cols="120" v-on:keyup="onChange" style="max-height: 80px; height: 80px; resize: none;" maxlength="255" v-model="descr.Description" class="form-control col-md-9" name="WorkTimeDetails[]" placeholder="Enter task"></textarea>
                                  </td>
                                  <td style="vertical-align: top;">
                                      <input type="number" style="width:100px" v-model="descr.Hours" class="form-control col-md-1" />
                                  </td>
                              </tr>
                            </div>
                            <div class="form-group row">
                              <button class="modal-default-button js-add btn btn-default" v-on:click="$emit('close');">Close</button>
                              <button class="modal-default-button js-add btn btn-default" v-on:click="onSave">Save </button>
                            </div>

                        </slot>
                    </div>

                    <div class="modal-footer">
                        <slot name="footer">
                        </slot>
                    </div>
                </div>
            </div>
        </div>
    </transition>

</template>

<script>
  

  import vselect from './ImportedComponents/vue-select.mod.min.js'
  // import vselect from './ImportedComponents/Select.vue'

  export default{
    props: ['WorkTimes', 'WorkTime', 'Persons', 'StartUrl'],
    data: function () {
      return {
        optionsSupp: this.Persons,
        optionsClient: this.Persons,
        persons: this.Persons
      }
    },
    components: {
      vselect
    },
    methods: {
      onChange (val) {
        // console.log("onChange");
        if (this.WorkTime.WorkTimeDetails[this.WorkTime.WorkTimeDetails.length - 1].Description.length) {
          this.WorkTime.WorkTimeDetails.push({ Description: '', Hours: 0.00 })
        }
      },
      getClientOptions (search, loading) {
        loading(true)
        this.options = this.persons.filter(function (item) { return item.Name.toLowerCase().indexOf(search.toLowerCase()) !== -1 })
        loading(false)
      },
      getClientIdCallback (val) {
        if (val==null) return;
        this.WorkTime.ClientId = val.Id
        // console.dir(JSON.stringify(val));
      },
      getSuppOptions (search, loading) {
        loading(true)
        this.options = this.persons.filter(function (item) { return item.Name.toLowerCase().indexOf(search.toLowerCase()) !== -1 })
        loading(false)
      },
      getSuppIdCallback (val) {
        if (val==null) return;
        this.WorkTime.SuppId = val.Id
        // console.dir(JSON.stringify(val));
      },
      onSave () {
        this.WorkTime.Client = this.persons.find((el) => el.Id === this.WorkTime.ClientId)
        this.WorkTime.ClientName = this.WorkTime.Client.Name
        this.WorkTime.Name = this.WorkTime.WorkTimeDetails[0].Description

        this.WorkTime.WorkTimeDetails.forEach((item) => { item.Hours = Number(item.Hours) })

        var d = this.WorkTime.Date
        d = d.slice(6, 10) + '-' + d.slice(3, 5) + '-' + d.slice(0, 2)

        var sendModel = {
          Id: this.WorkTime.Id,
          OpDate: d,
          ClientId: this.WorkTime.ClientId,
          SuppId: this.WorkTime.SuppId,
          WorkTimeDetails: this.WorkTime.WorkTimeDetails.filter(function (item) { return item.Description.trim() !== '' })
        }

        var xhr = new XMLHttpRequest()
        xhr.open('POST', this.StartUrl + 'AddOrUpdate', true)
        xhr.setRequestHeader('Content-type', 'application/json; charset=utf-8')

        // xhr.withCredentials = true;
        xhr.onreadystatechange = () => {
          if (xhr.readyState === 4 && xhr.status === 200) {
            var flag = this.WorkTime.Id === 0
            this.WorkTime.Id = Number(xhr.responseText)

            if (flag) this.WorkTimes.unshift(this.WorkTime)
            // window.history.pushState('/WorkTime/AddOrUpdate', '/WorkTime/AddOrUpdate', '/WorkTime/AddOrUpdate/'+this.WorkTime.Id);
            // console.log(xhr.responseText);
          }
        }
        var data = JSON.stringify(sendModel)
        xhr.send(data)
      }
    }
}
</script>




<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    .modal-mask {
        position: fixed;
        z-index: 9998;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, .5);
        display: table;
        transition: opacity .3s ease;
    }

    .modal-wrapper {
        display: table-cell;
        vertical-align: middle;
    }

    .modal-container {
        width: 1200px;
        margin: 0px auto;
        padding: 20px 30px;
        background-color: #fff;
        border-radius: 2px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, .33);
        transition: all .3s ease;
        font-family: Helvetica, Arial, sans-serif;
    }

    .modal-header h3 {
        margin-top: 0;
        color: #42b983;
    }

    .modal-body {
        margin: 20px 0;
    }

    .modal-default-button {
        float: right;
    }

    .modal-enter {
        opacity: 0;
    }

    .modal-leave-active {
        opacity: 0;
    }

    .modal-enter .modal-container,
    .modal-leave-active .modal-container {
            -webkit-transform: scale(1.1);
            transform: scale(1.1);
        }
</style>
