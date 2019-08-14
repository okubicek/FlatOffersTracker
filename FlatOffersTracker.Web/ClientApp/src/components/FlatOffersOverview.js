import React, { Component } from 'react';
import { FlatOfferCard } from './FlatOfferCard';

export class FlatOffersOverview extends Component {
    constructor(props) {
        super(props);

        this.state = { flatOffers : [], loading : true };

        fetch('api/FlatOffers/Get')
            .then(response => response.json())
            .then(data => {
                this.setState({ flatOffers : data, loading : false })
            });
    }

    renderFlatOffers(flatOffers) {
        return (
            flatOffers.map(offer =>
                <div className="col col-sm-4">
                    <FlatOfferCard flatOffer={offer} />
                </div>
            )
        );
    }

    render() {
        return this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderFlatOffers(this.state.flatOffers);
    }
}