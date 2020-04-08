import React, { Component } from 'react';
import { FlatOfferCard } from './FlatOfferCard';
import { PushSpinner } from "react-spinners-kit";

export class FlatOffersOverview extends Component {
    showMore() {        
        this.props.handleShowMore();
    }

    renderFlatOffers(flatOffers) {
        return (
            <React.Fragment>
                <div className="row">
                    {flatOffers.map(offer =>
                        <div className="col-12 col-sm-6 col-md-4">
                            <FlatOfferCard flatOffer={offer} />
                        </div>
                    )}
                </div>
                <div className="row mt-4 text-center">
                    <div className="col">
                        {this.props.isThereMorePagesToLoad
                            ? <button className="btn btn-primary" onClick={this.showMore}>Show More</button>
                            : null
                        }
                    </div>
                </div>
            </React.Fragment>
        );
    }

    renderSpinner() {
        return (
            <div className="row text-center">
                <div className="col">
                    <PushSpinner color="#ffffff" className="text-center" />
                </div>
            </div>
        );
    }

    render() {        
        return this.props.loading && this.props.flatOffers.length == 0
            ? this.renderSpinner()
            : this.renderFlatOffers(this.props.flatOffers);
    }
}