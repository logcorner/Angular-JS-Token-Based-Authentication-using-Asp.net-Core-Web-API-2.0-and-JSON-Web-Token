"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var router_1 = require("@angular/router");
var login_component_1 = require("../login/login.component");
var signup_component_1 = require("../signup/signup.component");
var auth_guard_1 = require("../common/auth.guard");
var shared_module_1 = require("../shared/shared.module");
var index_1 = require("./index");
var UserModule = (function () {
    function UserModule() {
    }
    return UserModule;
}());
UserModule = __decorate([
    core_1.NgModule({
        imports: [
            shared_module_1.SharedModule,
            router_1.RouterModule.forChild([
                { path: 'login', component: login_component_1.LoginComponent },
                { path: 'signup', component: signup_component_1.SignupComponent },
            ])
        ],
        declarations: [
            login_component_1.LoginComponent,
            signup_component_1.SignupComponent
        ],
        providers: [
            index_1.UserService,
            auth_guard_1.AuthGuard,
            index_1.UserProfile
        ]
    })
], UserModule);
exports.UserModule = UserModule;
//# sourceMappingURL=user.module.js.map