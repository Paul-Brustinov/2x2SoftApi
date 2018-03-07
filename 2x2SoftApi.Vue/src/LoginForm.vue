<template>
    <transition name="modal">
        <div class="modal-mask">
            <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css">
            <div class="modal-wrapper">
                <div class="modal-container">

                    <div class="modal-header">
                        <slot name="header">
                            Login form
                        </slot>
                    </div>

                    <div class="modal-body input-group">
                        <slot name="body">

                            <div class="form-group row">
                                <label class="control-label col-md-3" for="Login">Login</label>
                                <div class="col-md-9">
                                    <input id="Login" name="Login" v-model="login" class="form-control" />
                                </div>
                            </div>

                            <div class="form-group row">
                                <label class="control-label col-md-3" for="Password">Password</label>
                                <div class="col-md-9">
                                    <input id="Password" type="password" name="Password" v-model="password" class="form-control" />
                                </div>
                            </div>

                            <div class="form-group row">
                              <button class="modal-default-button js-add btn btn-default" v-on:click="OnLogin">Login </button>
                              <button class="modal-default-button js-add btn btn-default" v-on:click="$emit('close');">Close</button>
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
export default{
  data: function(){
    return{
      login: "",
      password: "",
      token: ""
    }
  },
  methods:{
    OnLogin(){
      var self = this;
      var xhttp = new XMLHttpRequest();
       //data: { grant_type: 'password', username: $("#i-login").val(), password: $("#i-password").val(), RememberMe: true },
      xhttp.open("POST", "http://localhost:53973/Token", true);
      xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
          //
          if (this.responseText != "") {
            var data = JSON.parse(this.responseText);
            sessionStorage.setItem('tokenKey', data.access_token);
            self.token = data.access_token;
            //console.log(data.access_token);
          }
        }
      }
      xhttp.send("grant_type=password&username=" + self.login + "&password=" + self.password +"&RememberMe=true" );
      //{ grant_type: 'password', username: self.login, password: self.password, RememberMe: true }
      //"grant_type=password&username=eeeeee%40ee.ee&password=eeeeee&RememberMe=true"
    }
  }
}
</script>