import { AttractionType } from "./AttractionType";

export interface AttractionModel {
  id: string;
  name: string;
  type: AttractionType;
  miniumAge: string;
  capacity: string;
  description: string;
  eventIds: string[];
  available: string;
}