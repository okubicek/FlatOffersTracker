import React, { Component } from 'react';

export class FlatOfferCard extends Component {
    render() {
        var flatOffer = this.props.flatOffer;
        return (
            <div className="card">
                <div className="card-body">
                    <h5 className="card-title">{flatOffer.address}</h5>
                    <ul className="list-group">
                        <li className="list-group-item">Size : {flatOffer.flatSize}</li>
                        <li className="list-group-item">Rooms : {flatOffer.numberOfRooms}</li>
                        <li className="list-group-item">Type : {flatOffer.flatType}</li>
                    </ul>
                    <a href={flatOffer.url}>{flatOffer.url}</a>
                    <h4 className="card-text float-right">{flatOffer.price.toLocaleString()} Kc</h4>
                </div>
            </div>
        )
    }

    format(num) {
        return  
    }
}