export interface TicketModel {
  id: string;
  type: string;
  Date: string;
  QrId: string;
  eventId?: string | null;
  VisitorId: string;
}