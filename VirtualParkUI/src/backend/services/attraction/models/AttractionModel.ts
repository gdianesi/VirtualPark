import { AttractionType } from "./AttractionType";

export interface AttractionModel {
  id: string;
  name: string;
  type?: AttractionType;
  description?: string;
}
