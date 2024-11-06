// src/types/types.ts
export interface Auction {
  id: number;
  img: string;
  name: string;
  priceStart: string;
  startDay: string;
  startTime: string;
  endDay: string;
}

export interface AuctionDetails {
  listAuctionID: number;
  image: string;
  nameAuction: string;
  moneyDeposit: number;
  description: string;
  startingPrice: number; // Use `number` if it's a numeric value
  statusAuction: string;
  startDay: string;
  startTime: string;
  endDay: string;
  endTime: string;
  numberofAuctionRounds: number;
  timePerLap: string;
  priceStep: number;
  paymentMethod: string;
  fileAuctioneer: string | null;
  signatureImg: string;
  tImage: {
    tImageId: number;
    fileAID: number;
    imange: string;
    fileAttachments: string | null;
  };
  countdowntime: string;
}