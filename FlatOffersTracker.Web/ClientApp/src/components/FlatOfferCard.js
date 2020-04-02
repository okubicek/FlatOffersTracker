import React, { Component } from 'react';

import { Notification } from './Notification'

export class FlatOfferCard extends Component {
    render() {
        var flatOffer = this.props.flatOffer;
        return (
            <div className="card mt-4">
                <img className="card-img-top" src={new URL("api/FlatOffers/HeaderImage/" + flatOffer.id, "https://" + window.location.host)} />
                <div className="card-body">
                    <div className="row px-2">
                        <h5 className="card-title">{flatOffer.address}</h5>
                    </div>
                    <div className="row">
                        <dl className="col px-2">
                        <dt className="col col-7 col-lg-4">Size:</dt><dd className="col col-5 col-lg-8">{flatOffer.flatSize}</dd>
                        <dt className="col col-7 col-lg-4">Rooms:</dt><dd className="col col-5 col-lg-8">{flatOffer.numberOfRooms}</dd>
                        <dt className="col col-7 col-lg-4">Type:</dt><dd className="col col-5 col-lg-8">{flatOffer.flatType}</dd>
                        </dl>
                    </div>
                    <div className="row"><div className="col">
                        <Notification notificationType={this.props.flatOffer.notificationType} />
                    </div></div>
                    <a href={flatOffer.url} target="_blank">Navigate to</a>
                    <h4 className="card-text float-md-right" target="_blank">{flatOffer.price.toLocaleString()} Kc</h4>
                </div>
            </div>
        )
    }
}