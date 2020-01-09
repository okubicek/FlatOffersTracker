﻿import React, { Component } from 'react';

export class FlatOfferCard extends Component {
    render() {
        var flatOffer = this.props.flatOffer;
        return (
            <div className="card mt-4">
                <img className="card-img-top" src={new URL("api/FlatOffers/HeaderImage/" + flatOffer.id, "https://" + window.location.host)} />
                <div className="card-body">
                    <h5 className="card-title">{flatOffer.address}</h5>
                    <dl className="row">
                        <dt className="col col-sm-4">Size:</dt><dd className="col col-sm-8">{flatOffer.flatSize}</dd>
                        <dt className="col col-sm-4">Rooms:</dt><dd className="col col-sm-8">{flatOffer.numberOfRooms}</dd>
                        <dt className="col col-sm-4">Type:</dt><dd className="col col-sm-8">{flatOffer.flatType}</dd>
                    </dl>
                    <a href={flatOffer.url} target="_blank">Navigate to</a>
                    <h4 className="card-text float-right" target="_blank">{flatOffer.price.toLocaleString()} Kc</h4>
                </div>
            </div>
        )
    }
}