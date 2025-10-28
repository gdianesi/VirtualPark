import { VisitorProfileModel } from "./VisitorProfileModel";
export interface UserModel {
    id: string;
    name: string;
    lastName: string;
    email: string;
    roles: RoleModel[];
    visitorProfile: VisitorProfileModel[];
}