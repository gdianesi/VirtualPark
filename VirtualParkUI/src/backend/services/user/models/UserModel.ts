import { VisitorProfileModel } from "./VisitorProfileModel";
import { RoleModel } from "./../../role/models/RoleModel";
export interface UserModel {
    id: string;
    name: string;
    lastName: string;
    email: string;
    roles: RoleModel[];
    visitorProfile: VisitorProfileModel[];
}